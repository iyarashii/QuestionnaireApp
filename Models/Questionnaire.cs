using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public class Questionnaire
    {
        public string Title { get; set; }
        public string Description { get; set; }

        // ostateczny termin wypelnienia
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        // TODO: ankieta ogolna gdy targets bedzie puste?
        public List<User> Targets { get; set; }

        public List<Question> Questions { get; set; }
    }
}
