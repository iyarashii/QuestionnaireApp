using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public class Questionnaire
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }
        public string Description { get; set; }

        // ostateczny termin wypelnienia
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Recipients")]
        public IList<QuestionnaireGroup> Targets { get; set; }

        public IList<Question> Questions { get; set; }
    }
}
