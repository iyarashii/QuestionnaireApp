using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using QuestionnaireApp.Models;

namespace QuestionnaireApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SignInManager<User> _signInManager;
        public bool IsAdmin =>
    HttpContext.User.HasClaim("IsAdmin", bool.TrueString);

        public IndexModel(ILogger<IndexModel> logger, SignInManager<User> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("./Questionnaires/Index");
            }
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
}
