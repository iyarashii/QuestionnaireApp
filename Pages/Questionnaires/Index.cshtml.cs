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

        public PaginatedList<Questionnaire> Questionnaires { get;set; }
        public string TitleSort { get; set; }
        public string DueDateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            // reset number of questions after questionnaire creation
            NumberOfQuestions = new List<int> { 2 };

            // sorting and searching logic
            CurrentSort = sortOrder;
            TitleSort = String.IsNullOrEmpty(sortOrder) ? "Title_desc" : "";
            DueDateSort = sortOrder == "DueDate" ? "DueDate_desc" : "DueDate";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            CurrentFilter = searchString;

            IQueryable<Questionnaire> questionnairesIQ = from all in _context.Questionnaires select all;

            if (!string.IsNullOrEmpty(searchString))
            {
                questionnairesIQ = questionnairesIQ.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Title_desc":
                    questionnairesIQ = questionnairesIQ.OrderByDescending(s => s.Title);
                    break;
                case "DueDate":
                    questionnairesIQ = questionnairesIQ.OrderBy(s => s.DueDate);
                    break;
                case "DueDate_desc":
                    questionnairesIQ = questionnairesIQ.OrderByDescending(s => s.DueDate);
                    break;
                default:
                    questionnairesIQ = questionnairesIQ.OrderBy(s => s.Title);
                    break;
            }
            int pageSize = 15;
            if(User.HasClaim(Constants.IsAdminClaim.Type, Constants.IsAdminClaim.Value))
            {
                pageSize = 6;
            }
            Questionnaires = await PaginatedList<Questionnaire>.CreateAsync(
                questionnairesIQ
                .Include(q => q.Targets)
                    .ThenInclude(g => g.Group)
                        .ThenInclude(g => g.UserGroups)
                        .AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
