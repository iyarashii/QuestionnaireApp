using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestionnaireApp.Models;
using QuestionnaireApp.Services;
using SelectPdf;

namespace QuestionnaireApp.Pages.Questionnaires
{
    public class ResultsModel : PageModel
    {
        private readonly QuestionnaireApp.Data.ApplicationDbContext _context;
        private ViewToStringRendererService _viewToStringRenderer;

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

        public async void OnPostAsync()
        {
            // Tutaj to trzeba wywołać, chyba ma tu być url i viewmodel do tego
            var html = await _viewToStringRenderer.RenderViewToStringAsync("/Questionnaires/Results/", null);
            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(html);
            doc.Save("raport.pdf");
            doc.Close();
        }

    }

    
}
