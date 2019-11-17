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

        public IList<Group> Group { get;set; }

        public async Task OnGetAsync()
        {
            var group = new Group();
            group.UserGroups = new List<UserGroup>();
            PopulateAssignedUserData(_context, group);

            Group = await _context.Groups.Include(g => g.UserGroups).ToListAsync();
        }
    }
}
