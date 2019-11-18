using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public class QuestionnaireGroup
    {
        public int GroupID { get; set; }
        public int QuestionnaireID { get; set; }
        public Group Group { get; set; }
        public Questionnaire Questionnaire { get; set; }
    }
}
