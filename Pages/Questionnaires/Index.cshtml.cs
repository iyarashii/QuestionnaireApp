using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestionnaireApp.Data;
using QuestionnaireApp.Models;

namespace QuestionnaireApp.Pages.Questionnaires
{
    public class IndexModel : QuestionnairesPageModel
    {
        private readonly QuestionnaireApp.Data.ApplicationDbContext _context;

        public IndexModel(QuestionnaireApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Questionnaire> Questionnaire { get;set; }

        public async Task OnGetAsync()
        {
            NumberOfQuestions = new List<int> { 2 };
            Questionnaire = await _context.Questionnaires.ToListAsync();
        }
    }
}
