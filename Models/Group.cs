using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public class Group
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IList<UserGroup> UserGroups { get; set; }
    }
}
