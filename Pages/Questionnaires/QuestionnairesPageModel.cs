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
using Microsoft.AspNetCore.Identity;

namespace QuestionnaireApp.Pages.Questionnaires
{
    public class QuestionnairesPageModel : PageModel
    {
        public List<QuestionnaireAssignedGroupData> QuestionnaireAssignedGroupsDataList;
        public List<List<SelectedAnswerData>> SelectedAnswerDataList = new List<List<SelectedAnswerData>>();

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

        public async Task PopulateSelectedAnswerData(Questionnaire questionnaire, UserManager<User> userManager)
        {
            User user = await userManager.GetUserAsync(User);
            for (int q = 0; q < questionnaire.Questions.Count; q++)
            {
                var questionAnswers = questionnaire.Questions[q].Answers;
                SelectedAnswerDataList.Add(new List<SelectedAnswerData>());

                for (int a = 0; a < questionnaire.Questions[q].Answers.Count; a++)
                {
                    var answerUsers = new HashSet<string>(
                    questionnaire.Questions[q].Answers[a].AnswerUsers.Select(au => au.UserID));
                    SelectedAnswerDataList[q].Add(new SelectedAnswerData
                    {
                        AnswerID = questionAnswers[a].ID,
                        Number = questionAnswers[a].Number,
                        Content = questionAnswers[a].Content,
                        Selected = answerUsers.Contains(user.Id)
                    });
                }
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


        public async Task UpdateUserAnswers(ApplicationDbContext context,
           List<string> selectedAnswers, Questionnaire questionnaireToUpdate, UserManager<User> userManager)
        {
            User user = await userManager.GetUserAsync(User);
            if (selectedAnswers == null)
            {
                for (int q = 0; q < questionnaireToUpdate.Questions.Count; q++)
                {
                    for (int a = 0; a < questionnaireToUpdate.Questions[q].Answers.Count; a++)
                    {
                        questionnaireToUpdate.Questions[q].Answers[a].AnswerUsers = new List<UserAnswer>();
                    }
                }
                return;
            }

            var selectedAnswersHS = new HashSet<string>(selectedAnswers);

            for (int q = 0; q < questionnaireToUpdate.Questions.Count; q++)
            {
                for (int a = 0; a < questionnaireToUpdate.Questions[q].Answers.Count; a++)
                {
                    var answerUsers = new HashSet<string>(
                    questionnaireToUpdate.Questions[q].Answers[a].AnswerUsers.Select(au => au.UserID));
                    if (selectedAnswersHS.Contains(questionnaireToUpdate.Questions[q].Answers[a].ID.ToString()))
                    {
                        if (!answerUsers.Contains(user.Id))
                        {
                            questionnaireToUpdate.Questions[q].Answers[a].AnswerUsers.Add(
                                new UserAnswer
                                {
                                    AnswerID = questionnaireToUpdate.Questions[q].Answers[a].ID,
                                    UserID = user.Id
                                });
                        }
                    }
                    else
                    {
                        if (answerUsers.Contains(user.Id))
                        {
                            UserAnswer userAnswerToRemove
                                = questionnaireToUpdate
                                    .Questions[q].Answers[a].AnswerUsers
                                    .SingleOrDefault(ua => ua.UserID == user.Id);
                            context.Remove(userAnswerToRemove);
                        }
                    }
                }
            }
        }
    }
}
