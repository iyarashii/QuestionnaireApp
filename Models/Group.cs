using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public class Group
    {
        public int ID { get; set; }
        // TODO: add something to validate that name is unique
        [Required]
        public string Name { get; set; }

        [Display(Name = "Members")]
        public IList<UserGroup> UserGroups { get; set; }

        // list of available questionnaires for the particular group
        public IList<QuestionnaireGroup> GroupQuestionnaires { get; set; }
    }
}
