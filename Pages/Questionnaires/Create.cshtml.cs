using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestionnaireApp.Data;
using QuestionnaireApp.Models;

namespace QuestionnaireApp.Pages.Questionnaires
{
    public class CreateModel : QuestionnairesPageModel
    {
        private readonly QuestionnaireApp.Data.ApplicationDbContext _context;

        public CreateModel(QuestionnaireApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? questions, int? answers)
        {
            var questionnaire = new Questionnaire();
            questionnaire.Targets = new List<QuestionnaireGroup>();

            if (questions != null && questions == NumberOfQuestions.Count)
            {
                AddQuestion();
            }
            if (answers != null)
            {
                AddAnswer(answers ?? 0);
            }

            PopulateAssignedQuestionnaireGroupData(_context, questionnaire);

            return Page();
        }


        [BindProperty]
        public Questionnaire Questionnaire { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string[] selectedGroups)
        {
            var newQuestionnaire = new Questionnaire();
            if (selectedGroups != null)
            {
                newQuestionnaire.Targets = new List<QuestionnaireGroup>();
                foreach (var group in selectedGroups)
                {
                    var groupToAdd = new QuestionnaireGroup
                    {
                        GroupID = int.Parse(group)
                    };
                    newQuestionnaire.Targets.Add(groupToAdd);
                }
            }

            if (await TryUpdateModelAsync<Questionnaire>(
                newQuestionnaire,
                "Questionnaire",
                q => q.Title, q => q.Description,
                q => q.DueDate, q => q.Questions, q => q.Targets))
            {
                _context.Questionnaires.Add(newQuestionnaire);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateAssignedQuestionnaireGroupData(_context, newQuestionnaire);

            return Page();
        }
    }
}
