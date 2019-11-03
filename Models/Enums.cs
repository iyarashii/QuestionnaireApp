using System;
using System.Collections.Generic;
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

    public enum Roles
    {
        Administrator
    };

    // TODO: wzorowane na google forms https://imgur.com/a/ship2mF
    public enum AnswerTypes
    {
        WrittenAnswer,
        MultipleChoice,
        Checkboxes,
        Dropdown
    };
}