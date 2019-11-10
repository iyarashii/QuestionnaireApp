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
        //public string Password { get; set; }
        //public override string Email { get; set; }
        //public override string PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        public Genders Gender { get; set; }

        // TODO: sprawdzic czy ma sens  
        // lista ID grup do ktorych nalezy user 
        public IList<Group> Groups { get; set; }
    }
}
