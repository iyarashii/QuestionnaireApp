using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models.ViewModels
{
    public class AssignedUserData
    {
        public string UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Assigned { get; set; }
    }
}
