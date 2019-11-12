using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestionnaireApp.Models;

namespace QuestionnaireApp.Pages.Users
{
    public class GrantAdminRightsModel : PageModel
    {
        private readonly QuestionnaireApp.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;


        public GrantAdminRightsModel(QuestionnaireApp.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            Claim IsAdminClaim = new Claim("IsAdmin", bool.TrueString);

            try
            {
                //var claims = await _userManager.GetClaimsAsync(user);

                // get all users with admin permissions
                var currentAdmins = await _userManager.GetUsersForClaimAsync(IsAdminClaim);

                // check if selected user has admin permissions if not grant him permissions
                if (!currentAdmins.Contains(user))
                {
                    await _userManager.AddClaimAsync(user, IsAdminClaim);
                                        //new Claim("IsAdmin", bool.TrueString));
                    await _context.SaveChangesAsync();
                }
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./GrantAdminRights",
                                     new { id, saveChangesError = true });
            }
        }
    }
}