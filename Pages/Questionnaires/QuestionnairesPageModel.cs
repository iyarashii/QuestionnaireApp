using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestionnaireApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using QuestionnaireApp.Data;
using QuestionnaireApp.Models.ViewModels;

namespace QuestionnaireApp.Pages.Questionnaires
{
    public class QuestionnairesPageModel : PageModel
    {
        public List<QuestionnaireAssignedGroupData> QuestionnaireAssignedGroupsDataList;

        public static List<int> NumberOfQuestions = new List<int> { 2 };
        public void AddQuestion()
        {
            NumberOfQuestions.Add(2);
        }
        public void AddAnswer(int answerIndex)
        {
            NumberOfQuestions[answerIndex] = NumberOfQuestions[answerIndex] + 1;
        }

        public void PopulateAssignedQuestionnaireGroupData(ApplicationDbContext context,
                                               Questionnaire questionnaire)
        {
            var allGroups = context.Groups;
            var questionnaireTargets = new HashSet<int>(
                questionnaire.Targets.Select(g => g.GroupID));
            QuestionnaireAssignedGroupsDataList = new List<QuestionnaireAssignedGroupData>();
            foreach (var group in allGroups)
            {
                QuestionnaireAssignedGroupsDataList.Add(new QuestionnaireAssignedGroupData
                {
                    GroupID = group.ID,
                    Name = group.Name,
                    Assigned = questionnaireTargets.Contains(group.ID)
                });
            }
        }
    }
}
