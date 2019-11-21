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
using Microsoft.AspNetCore.Mvc;

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

        public void UpdateQuestionnaireTargets(ApplicationDbContext context,
           string[] selectedGroups, Questionnaire questionnaireToUpdate)
        {
            if (selectedGroups == null)
            {
                questionnaireToUpdate.Targets = new List<QuestionnaireGroup>();
                return;
            }

            var selectedGroupsHS = new HashSet<string>(selectedGroups);
            var questionnaireTargets = new HashSet<int>
                (questionnaireToUpdate.Targets.Select(q => q.GroupID));
            foreach (var group in context.Groups)
            {
                if (selectedGroupsHS.Contains(group.ID.ToString()))
                {
                    if (!questionnaireTargets.Contains(group.ID))
                    {
                        questionnaireToUpdate.Targets.Add(
                            new QuestionnaireGroup
                            {
                                QuestionnaireID = questionnaireToUpdate.ID,
                                GroupID = group.ID
                            });
                    }
                }
                else
                {
                    if (questionnaireTargets.Contains(group.ID))
                    {
                        QuestionnaireGroup groupToRemove
                            = questionnaireToUpdate
                                .Targets
                                .SingleOrDefault(g => g.GroupID == group.ID);
                        context.Remove(groupToRemove);
                    }
                }
            }
        }
    }
}
