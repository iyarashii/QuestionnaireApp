using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestionnaireApp.Data;
using QuestionnaireApp.Models;

namespace QuestionnaireApp.Pages.Users
{
    public class IndexModel : UsersPageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string NameSort { get; set; }
        public string FirstNameSort { get; set; }
        public string EmailSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<User> Users { get; set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            FirstNameSort = sortOrder == "FirstName" ? "firstname_desc" : "FirstName";
            EmailSort = sortOrder == "Email" ? "email_desc" : "Email";

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

            // get userclaims table content as list
            UserClaims = await _context.UserClaims.ToListAsync();
            
            // dont show current admin in users list
            User user = await _userManager.GetUserAsync(User);
            usersIQ = usersIQ.Where(s => s.Id != user.Id);
            

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
                case "FirstName":
                    usersIQ = usersIQ.OrderBy(s => s.FirstName);
                    break;
                case "firstname_desc":
                    usersIQ = usersIQ.OrderByDescending(s => s.FirstName);
                    break;
                case "Email":
                    usersIQ = usersIQ.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    usersIQ = usersIQ.OrderByDescending(s => s.Email);
                    break;
                default:
                    usersIQ = usersIQ.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 6;
            Users = await PaginatedList<User>.CreateAsync(
                usersIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}