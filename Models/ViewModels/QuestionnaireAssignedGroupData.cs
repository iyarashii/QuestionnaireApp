using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models.ViewModels
{
    public class QuestionnaireAssignedGroupData
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}
