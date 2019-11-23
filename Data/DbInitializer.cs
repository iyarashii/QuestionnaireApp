using Microsoft.AspNetCore.Identity;
using QuestionnaireApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<User> userManager)
        {
            UserManager<User> _userManager = userManager;

            // Look for any users.
            if (!context.Users.Any())
            {
                // password for all dummy users
                string password = "aaaaaa";

                var users = new User[]
                {
                new User {FirstName = "Marcin", LastName = "Kowalski", Email = "a@a.com", PhoneNumber = "000000000", EmailConfirmed = true, Gender = Genders.Male },
                new User {FirstName = "Piotr", LastName = "Dydo", Email = "b@b.com", PhoneNumber = "111111111", EmailConfirmed = true, Gender = Genders.Male },
                new User {FirstName = "Marcin", LastName = "Hajdo", Email = "c@c.com", PhoneNumber = "222222222", EmailConfirmed = true, Gender = Genders.Male },
                new User {FirstName = "Konrad", LastName = "Dydo", Email = "d@d.com", PhoneNumber = "333333333", EmailConfirmed = true, Gender = Genders.Male },
                new User {FirstName = "Daniel", LastName = "Piskorski", Email = "e@e.com", PhoneNumber = "444444444", EmailConfirmed = true, Gender = Genders.Male },
                new User {FirstName = "Miyuki", LastName = "Sawashiro", Email = "f@f.com", PhoneNumber = "555555555", EmailConfirmed = true, Gender = Genders.Female },
                new User {FirstName = "Konomi", LastName = "Suzuki", Email = "g@g.com", PhoneNumber = "666666666", EmailConfirmed = true, Gender = Genders.Female },
                new User {FirstName = "Hisako", LastName = "Kanemoto", Email = "h@h.com", PhoneNumber = "77777777", EmailConfirmed = true, Gender = Genders.Female },
                new User {FirstName = "Yukari", LastName = "Tamura", Email = "i@i.com", PhoneNumber = "888888888", EmailConfirmed = true, Gender = Genders.Female },
                new User {FirstName = "Eri", LastName = "Kitamura", Email = "j@j.com", PhoneNumber = "999999999", EmailConfirmed = true, Gender = Genders.Female },
                new User {FirstName = "Test1", LastName = "Test1", Email = "k@k.com", PhoneNumber = "123123123", EmailConfirmed = true, Gender = Genders.Other },
                new User {FirstName = "Test2", LastName = "Test2", Email = "l@l.com", PhoneNumber = "456456456", EmailConfirmed = true, Gender = Genders.Other }
                };

                // set the email as username for every dummy user
                foreach (User u in users)
                {
                    u.UserName = u.Email;
                }

                // create users using password variable and users array
                foreach (User u in users)
                {
                    _userManager.CreateAsync(u, password).Wait();
                }

                // save created users to the database
                context.SaveChanges(); 
            }

            // Look for any groups.
            if (!context.Groups.Any())
            {
                // get all existing users form db context
                var allExistingUsers = context.Users;

                // create gender-based groups
                var groups = new Group[]
                {
                    new Group {Name = "Females"},
                    new Group {Name = "Males"},
                    new Group {Name = "Others"}
                };

                // query db for the users of each gender and then add them to the correct groups
                foreach (var group in groups)
                {
                    group.UserGroups = new List<UserGroup>();
                }
                IQueryable<string> femaleQuery = from u in allExistingUsers where u.Gender.Equals(Genders.Female) select u.Id;
                List<string> femaleIDs = femaleQuery.ToList();
                foreach (string id in femaleIDs)
                {
                    var userToAdd = new UserGroup
                    {
                        UserID = id
                    };
                    groups[0].UserGroups.Add(userToAdd);
                }

                IQueryable<string> maleQuery = from u in allExistingUsers where u.Gender.Equals(Genders.Male) select u.Id;
                List<string> maleIDs = maleQuery.ToList();
                foreach (string id in maleIDs)
                {
                    var userToAdd = new UserGroup
                    {
                        UserID = id
                    };
                    groups[1].UserGroups.Add(userToAdd);
                }

                IQueryable<string> otherQuery = from u in allExistingUsers where u.Gender.Equals(Genders.Other) select u.Id;
                List<string> otherIDs = otherQuery.ToList();
                foreach (string id in otherIDs)
                {
                    var userToAdd = new UserGroup
                    {
                        UserID = id
                    };
                    groups[2].UserGroups.Add(userToAdd);
                }

                // add groups to the db context
                foreach (var group in groups)
                {
                    context.Groups.Add(group);
                }
                // update db
                context.SaveChanges();
            }

            // look for any questionnaires
            if (!context.Questionnaires.Any())
            {
                // get all existing groups form db context
                var allExistingGroups = context.Groups;

                // create gender based questionnaires
                var questionnaires = new List<Questionnaire>
                {
                    new Questionnaire { Title = "For Men", Description = "Questionnaire targeted towards our male users.", DueDate = DateTime.Now.Date.AddDays(10) },
                    new Questionnaire { Title = "For Women", Description = "Integrating women into previously closed military occupations.", DueDate = DateTime.Now.Date.AddDays(10) },
                    new Questionnaire { Title = "For Others", Description = "Questionnaire targeted towards our other users.", DueDate = DateTime.Now.Date.AddDays(10) },
                    new Questionnaire { Title = "General Survey", Description = "Questionnaire targeted towards all users.", DueDate = DateTime.Now.Date.AddDays(10) }
                };

                foreach (var questionnaire in questionnaires)
                {
                    questionnaire.Targets = new List<QuestionnaireGroup>();
                    questionnaire.Questions = new List<Question>();
                }

                #region maleQuestionnaire
                var maleQuestions = new Question[]
                {
                    // q1
                    new Question { Content = "Do you shave?", QuestionType = QuestionTypes.SingleChoice, Number = 1,
                        Answers = new List<Answer>()
                        {
                            new Answer { Content = "Yes", Number = 1 },
                            new Answer { Content = "No", Number = 2 }
                        } },
                    // q2
                    new Question { Content = "Select brands of shaving foam that you use(leave blank if you don't use foam):", QuestionType = QuestionTypes.MultipleChoice, Number = 2,
                        Answers = new List<Answer>()
                        {
                            new Answer { Content = "NIVEA", Number = 1 },
                            new Answer { Content = "Gillette", Number = 2 },
                            new Answer { Content = "L'Oréal Paris", Number = 3 },
                            new Answer { Content = "Other", Number = 4 }
                        } },
                    // q3
                    new Question { Content = "Do you use aftershave?", QuestionType = QuestionTypes.SingleChoice, Number = 3,
                        Answers = new List<Answer>()
                        {
                            new Answer { Content = "Yes", Number = 1 },
                            new Answer { Content = "No", Number = 2 }
                        } },
                    // q4
                    new Question { Content = "Select brands of aftershave that you use(leave blank if you don't use aftershave):", QuestionType = QuestionTypes.MultipleChoice, Number = 4,
                        Answers = new List<Answer>()
                        {
                            new Answer { Content = "NIVEA", Number = 1 },
                            new Answer { Content = "adidas", Number = 2 },
                            new Answer { Content = "BOSS", Number = 3 },
                            new Answer { Content = "DAVIDOFF", Number = 4 },
                            new Answer { Content = "JOOP!", Number = 5 },
                            new Answer { Content = "DENIM", Number = 6 },
                            new Answer { Content = "Other", Number = 7 }
                        } },
                    // q5
                    new Question { Content = "Do you drink alcohol?", QuestionType = QuestionTypes.SingleChoice, Number = 5,
                        Answers = new List<Answer>()
                        {
                            new Answer { Content = "Yes", Number = 1 },
                            new Answer { Content = "No", Number = 2 }
                        } },
                    // q6
                    new Question { Content = "Select types of alcohol that you drink(leave blank if you don't drink alcohol):", QuestionType = QuestionTypes.MultipleChoice, Number = 6,
                        Answers = new List<Answer>()
                        {
                            new Answer { Content = "Beer", Number = 1 },
                            new Answer { Content = "Wine", Number = 2 },
                            new Answer { Content = "Sake", Number = 3 },
                            new Answer { Content = "Mead", Number = 4 },
                            new Answer { Content = "Liquor", Number = 5 },
                            new Answer { Content = "Spirit", Number = 6 },
                            new Answer { Content = "Other", Number = 7 }
                        } },
                };
                // get male group id
                int malesGroupID = allExistingGroups.Where(g => g.Name == "Males").Select(g => g.ID).FirstOrDefault();
                var groupToAdd = new QuestionnaireGroup
                {
                    GroupID = malesGroupID
                };
                // add group to the questionnaire targets
                questionnaires[0].Targets.Add(groupToAdd);

                // add generated questions to the questionnaire
                foreach (var q in maleQuestions)
                {
                    questionnaires[0].Questions.Add(q);
                }
                #endregion

                #region femaleQuestionnaire
                var femaleQuestions = new List<Question>
                {
                    // q1
                    new Question { Content = "How did you end up in this occupation/career field?", QuestionType = QuestionTypes.SingleChoice, Number = 1,
                        Answers = new List<Answer>()
                        {
                            new Answer { Content = "I was recruited for it", Number = 1 },
                            new Answer { Content = "I volunteered for it, and it was my first choice", Number = 2 },
                            new Answer { Content = "I volunteered for it, but it wasn’t my first choice", Number = 3 },
                            new Answer { Content = "I was assigned to it/it was the only job open to me", Number = 4 }
                        } },
                    // q2
                    new Question { Content = "Were you interested in serving in this occupation/career field?", QuestionType = QuestionTypes.SingleChoice, Number = 2,
                        Answers = new List<Answer>()
                        {
                            new Answer { Content = "Yes", Number = 1 },
                            new Answer { Content = "No", Number = 2 },
                            new Answer { Content = "I didn't care", Number = 3 }
                        } },
                    // q3
                    new Question { Content = "If yes, why?", QuestionType = QuestionTypes.MultipleChoice, Number = 3,
                        Answers = new List<Answer>()
                        {
                            new Answer { Content = "Pay/enlistment bonus", Number = 1 },
                            new Answer { Content = "Learn these job skills", Number = 2 },
                            new Answer { Content = "Thought it would be a promising career track", Number = 3 },
                            new Answer { Content = "Wanted to continue a family tradition in this field", Number = 4 },
                            new Answer { Content = "Thought this job would be more challenging than others", Number = 5 }
                        } },
                    // q4
                    new Question { Content = "What are your future plans?", QuestionType = QuestionTypes.SingleChoice, Number = 4,
                        Answers = new List<Answer>()
                        {
                            new Answer { Content = "I would like to stay in the service in this occupation/career field", Number = 1 },
                            new Answer { Content = "I would like to stay in the service but transfer into a different occupation/career field", Number = 2 },
                            new Answer { Content = "I would like to leave the service", Number = 3 }
                        } },
                    // q5
                    new Question { Content = "Has serving in this unit made you more or less interested in staying inthe military?", QuestionType = QuestionTypes.SingleChoice, Number = 5,
                        Answers = new List<Answer>()
                        {
                            new Answer { Content = "It has made me more interested in staying in", Number = 1 },
                            new Answer { Content = "It has made little difference", Number = 2 },
                            new Answer { Content = "It has made me less interested in staying in", Number = 3 }
                        } },
                    // q6
                    new Question { Content = "How do you rank your overall work performance compared to the others that youwork with?", QuestionType = QuestionTypes.SingleChoice, Number = 6,
                        Answers = new List<Answer>()
                        {
                            new Answer { Content = "Top 15%", Number = 1 },
                            new Answer { Content = "Above average", Number = 2 },
                            new Answer { Content = "Average", Number = 3 },
                            new Answer { Content = "Below average", Number = 4 },
                            new Answer { Content = "Bottom 15%", Number = 5 }
                        } }
                };
                // get male group id
                int femalesGroupID = allExistingGroups.Where(g => g.Name == "Females").Select(g => g.ID).FirstOrDefault();
                groupToAdd = new QuestionnaireGroup
                {
                    GroupID = femalesGroupID
                };
                // add group to the questionnaire targets
                questionnaires[1].Targets.Add(groupToAdd);

                // add generated questions to the questionnaire
                foreach (var q in femaleQuestions)
                {
                    questionnaires[1].Questions.Add(q);
                }
                #endregion

                #region othersQuestionnaire
                var othersQuestions = new List<Question>
                {
                    // q1
                    new Question { Content = "Czy posiadasz telefon komórkowy?", QuestionType = QuestionTypes.SingleChoice, Number = 1,
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "Tak", Number = 1 },
                            new Answer { Content = "Nie", Number = 2 }
                        } },
                    // q2
                    new Question { Content = "Jakiej marki jest Twój telefon?", QuestionType = QuestionTypes.SingleChoice, Number = 2,
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "Apple", Number = 1 },
                            new Answer { Content = "HTC", Number = 2 },
                            new Answer { Content = "Samsung", Number = 3 },
                            new Answer { Content = "Sony", Number = 4 },
                            new Answer { Content = "LG", Number = 5 },
                            new Answer { Content = "Xiaomi", Number = 6 },
                            new Answer { Content = "Huawei", Number = 7 },
                            new Answer { Content = "Inna", Number = 8 }
                        } },
                    // q3
                    new Question { Content = "Do jakich czynności najczęściej wykorzystujesz swój telefon?", QuestionType = QuestionTypes.MultipleChoice, Number = 3,
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "Dzwonienie", Number = 1 },
                            new Answer { Content = "SMSy", Number = 2 },
                            new Answer { Content = "E-mail", Number = 3 },
                            new Answer { Content = "Przeglądanie stron www", Number = 4 },
                            new Answer { Content = "Robienie zdjęć i kręcenie filmów", Number = 5 },
                            new Answer { Content = "Granie", Number = 6 },
                            new Answer { Content = "Słuchanie muzyki/radia", Number = 7 }
                        } },
                    // q4
                    new Question { Content = "Które z poniższych czynników są dla Ciebie ważne przy podejmowaniu decyzji o zakupie telefonu?", QuestionType = QuestionTypes.MultipleChoice, Number = 4,
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "Wygląd", Number = 1 },
                            new Answer { Content = "Cena", Number = 2 },
                            new Answer { Content = "Funkcjonalności", Number = 3 },
                            new Answer { Content = "Marka", Number = 4 },
                            new Answer { Content = "Jakość wykonania", Number = 5 },
                        } },
                    // q5
                    new Question { Content = "Wielkość miejsca zamieszkania", QuestionType = QuestionTypes.SingleChoice, Number = 5,
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "wieś", Number = 1 },
                            new Answer { Content = "miasto do 20 tys. mieszkańców", Number = 2 },
                            new Answer { Content = "miasto od 20 tys. do 100 tys. mieszkańców", Number = 3 },
                            new Answer { Content = "miasto od 100 tys. do 500 tys. mieszkańców", Number = 4 },
                            new Answer { Content = "miasto pow. 500 tys. mieszkańców", Number = 5 }
                        } },
                    // q6
                    new Question { Content = "Czy jesteś osobą pełnoletnią?", QuestionType = QuestionTypes.SingleChoice, Number = 6,
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "Tak", Number = 1 },
                            new Answer { Content = "Nie", Number = 2 }
                        } }
                };
                // get male group id
                int otherssGroupID = allExistingGroups.Where(g => g.Name == "Others").Select(g => g.ID).FirstOrDefault();
                groupToAdd = new QuestionnaireGroup
                {
                    GroupID = otherssGroupID
                };
                // add group to the questionnaire targets
                questionnaires[2].Targets.Add(groupToAdd);

                // add generated questions to the questionnaire
                foreach (var q in othersQuestions)
                {
                    questionnaires[2].Questions.Add(q);
                }
                #endregion

                #region generalQuestionnaire
                var generalQuestions = new List<Question>
                {
                    // q1
                    new Question { Content = "Do you play video games?", QuestionType = QuestionTypes.SingleChoice, Number = 1,
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "Yes", Number = 1 },
                            new Answer { Content = "No", Number = 2 }
                        } },
                    // q2
                    new Question { Content = "What game genres do you enjoy the most?(leave blank if you don't play games)", QuestionType = QuestionTypes.MultipleChoice, Number = 2,
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "Action", Number = 1 },
                            new Answer { Content = "Action-adventure", Number = 2 },
                            new Answer { Content = "Adventure", Number = 3 },
                            new Answer { Content = "Role-playing", Number = 4 },
                            new Answer { Content = "Simulation", Number = 5 },
                            new Answer { Content = "Strategy", Number = 6 },
                            new Answer { Content = "Sports", Number = 7 }
                        } },
                    // q3
                    new Question { Content = "Do you listen to music?", QuestionType = QuestionTypes.SingleChoice, Number = 3,
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "Yes", Number = 1 },
                            new Answer { Content = "No", Number = 2 }
                        } },
                    // q4
                    new Question { Content = "What music genres do you listen to?(leave blank if you don't listen to music)", QuestionType = QuestionTypes.MultipleChoice, Number = 4,
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "Metal", Number = 1 },
                            new Answer { Content = "Rock", Number = 2 },
                            new Answer { Content = "Pop", Number = 3 },
                            new Answer { Content = "Classical", Number = 4 },
                            new Answer { Content = "Country", Number = 5 },
                            new Answer { Content = "Jazz", Number = 6 },
                            new Answer { Content = "Blues", Number = 7 },
                            new Answer { Content = "Techno", Number = 8 },
                            new Answer { Content = "Reggae", Number = 9 },
                            new Answer { Content = "Electronic dance music", Number = 10 },
                            new Answer { Content = "Disco", Number = 11 }
                        } },
                    // q5
                    new Question { Content = "Do you watch anime?", QuestionType = QuestionTypes.SingleChoice, Number = 5,
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "Yes", Number = 1 },
                            new Answer { Content = "No", Number = 2 }
                        } },
                    // q6
                    new Question { Content = "Select your favourite anime genres(leave blank if you don't watch anime):", QuestionType = QuestionTypes.MultipleChoice, Number = 6,
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "Shounen", Number = 1 },
                            new Answer { Content = "Shoujo", Number = 2 },
                            new Answer { Content = "Comedy", Number = 3 },
                            new Answer { Content = "Action", Number = 4 },
                            new Answer { Content = "Adventure", Number = 5 },
                            new Answer { Content = "Fantasy", Number = 6 },
                            new Answer { Content = "Sports", Number = 7 },
                            new Answer { Content = "School", Number = 8 },
                            new Answer { Content = "Super Power", Number = 9 },
                            new Answer { Content = "Romance", Number = 10 },
                            new Answer { Content = "Josei", Number = 11 }
                        } }
                };
                // get all group ids
                List<int> allGroupIDs = allExistingGroups.Select(g => g.ID).ToList();
                foreach (var id in allGroupIDs)
                {
                    groupToAdd = new QuestionnaireGroup
                    {
                        GroupID = id
                    };
                    // add group to the questionnaire targets
                    questionnaires[3].Targets.Add(groupToAdd);
                }

                // add generated questions to the questionnaire
                foreach (var q in generalQuestions)
                {
                    questionnaires[3].Questions.Add(q);
                }
                #endregion

                // add questionnaires to the db context
                foreach (var q in questionnaires)
                {
                    context.Questionnaires.Add(q);
                }
                // update db
                context.SaveChanges();
            }

        }
    }
}
