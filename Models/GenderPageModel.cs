using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Models
{
    public class GenderPageModel : PageModel
    {
        //public static List<Genders> GenderList = new List<Genders>() { Genders.Female, Genders.Male, Genders.Other };
        public SelectList GenderSL { get; set; } = new SelectList(new List<Genders> { Genders.Female, Genders.Male, Genders.Other });
    }
}
