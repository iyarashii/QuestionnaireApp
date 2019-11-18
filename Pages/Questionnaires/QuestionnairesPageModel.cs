using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestionnaireApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace QuestionnaireApp.Pages.Questionnaires
{
    public class QuestionnairesPageModel : PageModel
    {
        public static List<int> NumberOfQuestions = new List<int> { 2 };
        public void AddQuestion()
        {
            NumberOfQuestions.Add(2);
            //return NumberOfQuestions.Count;
        }
        public void AddAnswer(int answerIndex)
        {
            NumberOfQuestions[answerIndex] = NumberOfQuestions[answerIndex] + 1;
        }
    }
}
