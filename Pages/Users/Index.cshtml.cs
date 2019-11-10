using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestionnaireApp.Data;
using QuestionnaireApp.Models;

namespace QuestionnaireApp.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<User> Users { get; set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<User> usersIQ = from s in _context.Users
                                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                usersIQ = usersIQ.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    usersIQ = usersIQ.OrderByDescending(s => s.LastName);
                    break;
                //case "Date":
                //    usersIQ = usersIQ.OrderBy(s => s.EnrollmentDate);
                //    break;
                //case "date_desc":
                //    usersIQ = usersIQ.OrderByDescending(s => s.EnrollmentDate);
                //    break;
                default:
                    usersIQ = usersIQ.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            Users = await PaginatedList<User>.CreateAsync(
                usersIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}