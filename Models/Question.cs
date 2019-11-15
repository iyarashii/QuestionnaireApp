using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public class Question
    {
        // tresc pytania
        public string Content { get; set; }
        // typ odp jednokrotnego, wyboru wielokrotnego itd.
        public AnswerTypes AnswerType { get; set; }
        public IList<string> Answers { get; set; }
    }
}
