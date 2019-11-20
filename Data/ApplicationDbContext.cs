using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuestionnaireApp.Models;

namespace QuestionnaireApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // create database tables
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<QuestionnaireGroup> QuestionnaireGroups { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // alternative way to set attributes to models by Fluent API 
            // https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/complex-data-model?view=aspnetcore-3.0&tabs=visual-studio#fluent-api-alternative-to-attributes
            base.OnModelCreating(modelBuilder);
            // TODO: tried to make group name unique with this line of code but it does not work, idk why
            //modelBuilder.Entity<Group>().HasIndex(g => g.Name).IsUnique(true);

            // create composite primary keys for joined tables
            modelBuilder.Entity<UserGroup>()
                .HasKey(u => new { u.UserID, u.GroupID });

            modelBuilder.Entity<QuestionnaireGroup>()
                .HasKey(q => new { q.QuestionnaireID, q.GroupID });

            modelBuilder.Entity<UserAnswer>()
                .HasKey(ua => new { ua.UserID, ua.AnswerID });
        }
    }
}
