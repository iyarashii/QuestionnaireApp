using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public class Question
    {
        public int ID { get; set; }
        public int QuestionnaireID { get; set; }
        public Questionnaire Questionnaire { get; set; }

        // question contents
        [Required]
        [Display(Name = "Question")]
        public string Content { get; set; }

        // multiple choice etc.
        [Required]
        [Display(Name = "Question Type")]
        public QuestionTypes QuestionType { get; set; }
        public IList<Answer> Answers { get; set; }
    }
}
