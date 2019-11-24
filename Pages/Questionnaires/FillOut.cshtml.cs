using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestionnaireApp.Data;
using QuestionnaireApp.Models;

namespace QuestionnaireApp.Pages.Questionnaires
{
    public class FillOutModel : QuestionnairesPageModel
    {
        private readonly QuestionnaireApp.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public FillOutModel(QuestionnaireApp.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Questionnaire Questionnaire { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            // TODO: do something to prevent access by not targeted users from url
            // my hacky solution
            if(!User.HasClaim(Constants.IsAdminClaim.Type, Constants.IsAdminClaim.Value))
            {
                #region myhackysolution
                bool isUserAllowed = false;
                User user = await _userManager.GetUserAsync(User);
                var targets = await _context.Questionnaires
                    .Include(q => q.Targets)
                        .ThenInclude(g => g.Group)
                            .ThenInclude(g => g.UserGroups)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(q => q.ID == id);
                foreach (var qgroup in targets.Targets)
                {
                    if (qgroup.Group.UserGroups.Select(ug => ug.UserID).Contains(user.Id))
                    {
                        isUserAllowed = true;
                    }
                }
                if (!isUserAllowed)
                {
                    return RedirectToPage("/Error");
                }
                #endregion
            }

            Questionnaire = await _context.Questionnaires
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                        .ThenInclude(q => q.AnswerUsers)
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.ID == id);

            if (Questionnaire == null)
            {
                return NotFound();
            }
            // sort questions by question number
            Questionnaire.Questions = Questionnaire.Questions.OrderBy(q => q.Number).ToList();

            await PopulateSelectedAnswerData(Questionnaire, _userManager);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, List<string> selectedAnswers, string r0, string r1, string r2, string r3, string r4, string r5, string r6, string r7, string r8, string r9)
        {
            if (id == null)
            {
                return NotFound();
            }
            selectedAnswers.Add(r0);
            selectedAnswers.Add(r1);
            selectedAnswers.Add(r2);
            selectedAnswers.Add(r3);
            selectedAnswers.Add(r4);
            selectedAnswers.Add(r5);
            selectedAnswers.Add(r6);
            selectedAnswers.Add(r7);
            selectedAnswers.Add(r8);
            selectedAnswers.Add(r9);

            var questionnaireToUpdate = await _context.Questionnaires
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                        .ThenInclude(q => q.AnswerUsers)
                .FirstOrDefaultAsync(q => q.ID == id);

            if (questionnaireToUpdate == null)
            {
                return NotFound();
            }

            //if (await TryUpdateModelAsync<Questionnaire>(
            //    questionnaireToUpdate,
            //    "Questionnaire",
            //    q => q.Questions))
            if(selectedAnswers != null)
            {
                await UpdateUserAnswers(_context, selectedAnswers, questionnaireToUpdate, _userManager);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            await UpdateUserAnswers(_context, selectedAnswers, questionnaireToUpdate, _userManager);
            await PopulateSelectedAnswerData(questionnaireToUpdate, _userManager);
            return Page();
        }
    }
}
