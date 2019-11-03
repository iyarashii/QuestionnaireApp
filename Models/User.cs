using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Genders Gender { get; set; }

        // TODO: nie wiem czy robimy role czy dajemy IsAdmin bool?
        public Roles Role { get; set; }

        // TODO: sprawdzic czy ma sens  
        // lista ID grup do ktorych nalezy user 
        public List<int> Groups { get; set; }
        //public bool IsActive { get; set; }
        //public string ActivationCode { get; set; }
    }
}
