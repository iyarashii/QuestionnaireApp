using QuestionnaireApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {

            // Look for any students.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                //new User {FirstName = "Admin", LastName = "Admin", Email = "admin@admin.com", EmailConfirmed = true }
            };

            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            
            context.SaveChanges();
        }
    }
}
