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

        public void PopulateAssignedUserData(ApplicationDbContext context,
                                               Group group)
        {
            //List<string> adminIds = new List<string>();

            //var claims = context.UserClaims.Where(c => c.ClaimValue == bool.TrueString);
            //foreach (var claim in claims)
            //{
            //    adminIds.Add(claim.UserId);
            //}

            var allUsers = context.Users;
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

        public void UpdateGroupUsers(ApplicationDbContext context,
            string[] selectedUsers, Group groupToUpdate)
        {
            if (selectedUsers == null)
            {
                groupToUpdate.UserGroups = new List<UserGroup>();
                return;
            }

            var selectedUsersHS = new HashSet<string>(selectedUsers);
            var groupUsers = new HashSet<string>
                (groupToUpdate.UserGroups.Select(u => u.UserID));
            foreach (var user in context.Users)
            {
                if (selectedUsersHS.Contains(user.Id))
                {
                    if (!groupUsers.Contains(user.Id))
                    {
                        groupToUpdate.UserGroups.Add(
                            new UserGroup
                            {
                                GroupID = groupToUpdate.ID,
                                UserID = user.Id
                            });
                    }
                }
                else
                {
                    if (groupUsers.Contains(user.Id))
                    {
                        UserGroup userToRemove
                            = groupToUpdate
                                .UserGroups
                                .SingleOrDefault(i => i.UserID == user.Id);
                        context.Remove(userToRemove);
                    }
                }
            }
        }

    }
}
