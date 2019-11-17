using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestionnaireApp.Data;
using QuestionnaireApp.Models;

namespace QuestionnaireApp.Pages.Groups
{
    public class CreateModel : GroupsPageModel
    {
        private readonly QuestionnaireApp.Data.ApplicationDbContext _context;

        public CreateModel(QuestionnaireApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var group = new Group();
            group.UserGroups = new List<UserGroup>();

            PopulateAssignedUserData(_context, group);

            return Page();
        }

        [BindProperty]
        public Group Group { get; set; }

        public async Task<IActionResult> OnPostAsync(string[] selectedUsers)
        {
            var newGroup = new Group();
            if (selectedUsers != null)
            {
                newGroup.UserGroups = new List<UserGroup>();
                foreach (var user in selectedUsers)
                {
                    var userToAdd = new UserGroup
                    {
                       UserID = user
                    };
                    newGroup.UserGroups.Add(userToAdd);
                }
            }

            if (await TryUpdateModelAsync<Group>(
                newGroup,
                "Group",
                i => i.Name))
            {
                _context.Groups.Add(newGroup);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateAssignedUserData(_context, newGroup);
            return Page();
        }
    }
}
