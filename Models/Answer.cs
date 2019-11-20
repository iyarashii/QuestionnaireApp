using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public class Answer
    {
        // answer contents
        [Required]
        [Display(Name = "Answer")]
        public string Content { get; set; }
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public Question Question { get; set; }
        public IList<UserAnswer> AnswerUsers { get; set; }
    }
}
