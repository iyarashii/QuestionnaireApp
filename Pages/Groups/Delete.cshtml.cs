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
    public class DeleteModel : GroupsPageModel
    {
        private readonly QuestionnaireApp.Data.ApplicationDbContext _context;

        public DeleteModel(QuestionnaireApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Group Group { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = new Group
            {
                UserGroups = new List<UserGroup>()
            };

            PopulateAssignedUserData(_context, group);

            Group = await _context.Groups
                .Include(g => g.UserGroups)
                .FirstOrDefaultAsync(g => g.ID == id);

            if (Group == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Group = await _context.Groups.FindAsync(id);

            if (Group != null)
            {
                _context.Groups.Remove(Group);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
