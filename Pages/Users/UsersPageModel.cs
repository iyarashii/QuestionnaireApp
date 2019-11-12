using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Pages.Users
{
    public class UsersPageModel : PageModel
    {
        [Display(Name = "Admin Claims")]
        public List<IdentityUserClaim<string>> UserClaims { get; set; }
    }
}
