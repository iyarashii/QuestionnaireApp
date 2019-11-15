using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public class User : IdentityUser
    {
        // these properties are included in IdentityUser class
        //public override string UserName { get; set; }
        //public override string Email { get; set; }

        // override just to add display attribute
        [Display(Name = "Phone Number")]
        public override string PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public Genders Gender { get; set; }

        // list of the groups that this user is a part of
        public IList<UserGroup> UserGroups { get; set; }
    }
}
