using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestionnaireApp.Data;
using QuestionnaireApp.Models;

namespace QuestionnaireApp.Pages.Groups
{
    public class IndexModel : GroupsPageModel
    {
        private readonly QuestionnaireApp.Data.ApplicationDbContext _context;

        public IndexModel(QuestionnaireApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public string NameSort { get; set; }
        public string MemberCountSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Group> Groups { get;set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            MemberCountSort = sortOrder == "MemberCount" ? "MemberCount_desc" : "MemberCount";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            #region for group members display
            var group = new Group();
            group.UserGroups = new List<UserGroup>();
            PopulateAssignedUserData(_context, group);
            #endregion

            IQueryable<Group> groupsIQ = from all in _context.Groups select all;

            if (!String.IsNullOrEmpty(searchString))
            {
                groupsIQ = groupsIQ.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    groupsIQ = groupsIQ.OrderByDescending(s => s.Name);
                    break;
                case "MemberCount":
                    groupsIQ = groupsIQ.OrderBy(s => s.UserGroups.Count);
                    break;
                case "MemberCount_desc":
                    groupsIQ = groupsIQ.OrderByDescending(s => s.UserGroups.Count);
                    break;
                default:
                    groupsIQ = groupsIQ.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 3;
            Groups = await PaginatedList<Group>.CreateAsync(
                groupsIQ.Include(g => g.UserGroups).AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
