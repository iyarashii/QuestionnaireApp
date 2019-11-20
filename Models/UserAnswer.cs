using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public class UserAnswer
    {
        public string UserID { get; set; }
        public int AnswerID { get; set; }
        public User User { get; set; }
        public Answer Answer { get; set; }
    }
}
