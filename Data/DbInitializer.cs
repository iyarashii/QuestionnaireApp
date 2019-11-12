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
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

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
    }
}
