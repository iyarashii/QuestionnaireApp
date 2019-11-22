using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public enum Genders
    {
        Female,
        Male,
        Other
    };

    // TODO: wzorowane na google forms https://imgur.com/a/ship2mF
    public enum QuestionTypes
    {
        //WrittenAnswer,
        [Display(Name = "Multiple choice")]
        MultipleChoice,
        [Display(Name = "Single choice")]
        SingleChoice,
    };
}