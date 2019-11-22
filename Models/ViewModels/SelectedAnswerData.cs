using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models.ViewModels
{
    public class SelectedAnswerData
    {
        public int AnswerID { get; set; }
        public string Content { get; set; }
        public bool Selected { get; set; }
    }
}
