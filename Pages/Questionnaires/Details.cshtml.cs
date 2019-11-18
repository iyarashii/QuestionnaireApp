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
    public class DetailsModel : PageModel
    {
        private readonly QuestionnaireApp.Data.ApplicationDbContext _context;

        public DetailsModel(QuestionnaireApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Questionnaire Questionnaire { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Questionnaire = await _context.Questionnaires.FirstOrDefaultAsync(m => m.ID == id);

            if (Questionnaire == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
