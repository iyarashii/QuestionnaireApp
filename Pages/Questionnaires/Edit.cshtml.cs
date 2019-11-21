using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestionnaireApp.Data;
using QuestionnaireApp.Models;

namespace QuestionnaireApp.Pages.Questionnaires
{
    public class EditModel : QuestionnairesPageModel
    {
        private readonly QuestionnaireApp.Data.ApplicationDbContext _context;

        public EditModel(QuestionnaireApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Questionnaire Questionnaire { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Questionnaire = await _context.Questionnaires
                .Include(q => q.Targets)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.ID == id);

            if (Questionnaire == null)
            {
                return NotFound();
            }

            PopulateAssignedQuestionnaireGroupData(_context, Questionnaire);

            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedGroups)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionnaireToUpdate = await _context.Questionnaires
                .Include(q => q.Targets)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.ID == id);

            if (questionnaireToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Questionnaire>(
                questionnaireToUpdate,
                "Questionnaire",
                q => q.Title, q => q.Description,
                q => q.DueDate, q => q.Targets,
                q => q.Questions))
            {
                UpdateQuestionnaireTargets(_context, selectedGroups, questionnaireToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            UpdateQuestionnaireTargets(_context, selectedGroups, questionnaireToUpdate);
            PopulateAssignedQuestionnaireGroupData(_context, questionnaireToUpdate);
            return Page();
        }
    }
}
