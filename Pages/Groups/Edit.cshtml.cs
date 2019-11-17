using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestionnaireApp.Data;
using QuestionnaireApp.Models;

namespace QuestionnaireApp.Pages.Groups
{
    public class EditModel : GroupsPageModel
    {
        private readonly QuestionnaireApp.Data.ApplicationDbContext _context;

        public EditModel(QuestionnaireApp.Data.ApplicationDbContext context)
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

            Group = await _context.Groups
                .Include(g => g.UserGroups)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Group == null)
            {
                return NotFound();
            }

            PopulateAssignedUserData(_context, Group);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedUsers)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupToUpdate = await _context.Groups
                .Include(g => g.UserGroups)
                .FirstOrDefaultAsync(g => g.ID == id);

            if (groupToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Group>(
                groupToUpdate,
                "Group",
                i => i.Name))
            {
                UpdateGroupUsers(_context, selectedUsers, groupToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            UpdateGroupUsers(_context, selectedUsers, groupToUpdate);
            PopulateAssignedUserData(_context, groupToUpdate);
            return Page();
        }
    }
}
