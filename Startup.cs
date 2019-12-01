using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using QuestionnaireApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuestionnaireApp.Models;
using QuestionnaireApp.Services;

namespace QuestionnaireApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Source: https://dotnetstories.com/blog/Generate-a-HTML-string-from-cshtml-razor-view-using-ASPNET-Core-that-can-be-used-in-the-c-controlle-7173969632

            services.AddViewToStringRendererService();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });
            // add authorization
            services.AddAuthorization(options =>
                    options.AddPolicy("Admin", policy =>
                            policy.RequireAuthenticatedUser()
                            .RequireClaim("IsAdmin", bool.TrueString)));

            // authorize users folder so that only admin can browse /users/*
            services.AddMvc()
                    .AddRazorPagesOptions(options =>
                        options.Conventions.AuthorizeFolder("/Users/", "Admin"));

            // authorize groups folder
            services.AddMvc()
                    .AddRazorPagesOptions(options =>
                        options.Conventions.AuthorizeFolder("/Groups/", "Admin"));

            // prevent non-admin users from creating, deleting and editing questionnaires
            services.AddMvc()
                    .AddRazorPagesOptions(options =>
                    options.Conventions.AuthorizePage("/Questionnaires/Create", "Admin"));
            services.AddMvc()
                    .AddRazorPagesOptions(options =>
                    options.Conventions.AuthorizePage("/Questionnaires/Delete", "Admin"));
            services.AddMvc()
                    .AddRazorPagesOptions(options =>
                    options.Conventions.AuthorizePage("/Questionnaires/Edit", "Admin"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
