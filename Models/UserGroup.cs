using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public class UserGroup
    {
        public string UserID { get; set; }
        public int GroupID { get; set; }
        public User User { get; set; }
        public Group Group { get; set; }
    }
}
