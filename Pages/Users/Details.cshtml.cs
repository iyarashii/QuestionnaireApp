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
    public class DetailsModel : UsersPageModel
    {
        private readonly QuestionnaireApp.Data.ApplicationDbContext _context;

        public DetailsModel(QuestionnaireApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public User SelectedUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SelectedUser = await _context.Users
                //.Include(s => s.Enrollments)
                //.ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (SelectedUser == null)
            {
                return NotFound();
            }
            UserClaims = await _context.UserClaims.ToListAsync();

            return Page();
        }
    }
}
