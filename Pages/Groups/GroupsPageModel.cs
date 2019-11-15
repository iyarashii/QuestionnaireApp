using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestionnaireApp.Models.ViewModels;
using QuestionnaireApp.Data;
using QuestionnaireApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuestionnaireApp.Pages.Groups
{
    public class GroupsPageModel : PageModel
    {
        public List<AssignedUserData> AssignedUserDataList;
        public List<User> Users { get; set; }

        public void PopulateAssignedUserData(ApplicationDbContext context,
                                               Group group)
        {
            List<string> adminIds = new List<string>();

            var claims = context.UserClaims.Where(c => c.ClaimValue == bool.TrueString);
            foreach (var claim in claims)
            {
                adminIds.Add(claim.UserId);
            }

            var allUsers = context.Users.Where(u => !adminIds.Contains(u.Id));
            //var allUsers = context.Users;
            var groupUsers = new HashSet<string>(
                group.UserGroups.Select(u => u.UserID));
            AssignedUserDataList = new List<AssignedUserData>();
            foreach (var user in allUsers)
            {
                AssignedUserDataList.Add(new AssignedUserData
                {
                    UserID = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Assigned = groupUsers.Contains(user.Id)
                });
            }
        }
    }
}
