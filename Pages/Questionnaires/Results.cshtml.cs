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
    public class ResultsModel : PageModel
    {
        private readonly QuestionnaireApp.Data.ApplicationDbContext _context;

        public ResultsModel(QuestionnaireApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Questionnaire Questionnaire { get; set; }
        public List<UserAnswer> UserAnswers { get; set; }
        //public List<int> TargetedGroupsIDs { get; set; }
        public List<string> TargetedUsersIDs { get; set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Questionnaire = await _context.Questionnaires
               .Include(q => q.Targets)
                    .ThenInclude(t => t.Group)
                        .ThenInclude(g => g.UserGroups)
               .Include(q => q.Questions)
                   .ThenInclude(q => q.Answers)
                       .ThenInclude(q => q.AnswerUsers)
               .AsNoTracking()
               .FirstOrDefaultAsync(q => q.ID == id);

            //List<int> TargetedGroupsIDs = Questionnaire.Targets.Select(t => t.GroupID).ToList();
            //List<Group> TargetedGroups =   await _context.Groups.Include(g => g.UserGroups).Where(g => TargetedGroupsIDs.Contains(g.ID)).ToListAsync();
            foreach(var qgroup in Questionnaire.Targets)
            {
                foreach (var user in qgroup.Group.UserGroups)
                {
                    if (!TargetedUsersIDs.Contains(user.UserID))
                    {
                        TargetedUsersIDs.Add(user.UserID);
                    }
                }
            }

            UserAnswers = await _context.UserAnswers.AsNoTracking().ToListAsync();

            if (Questionnaire == null)
            {
                return NotFound();
            }

            // sort questions
            Questionnaire.Questions = Questionnaire.Questions.OrderBy(q => q.Number).ToList();

            return Page();
        }
    }
}
