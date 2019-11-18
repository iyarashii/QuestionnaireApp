using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuestionnaireApp
{
    public static class Constants
    {
        public static readonly Claim IsAdminClaim = new Claim("IsAdmin", bool.TrueString);
        public static readonly Claim IsNotAdminClaim = new Claim("IsAdmin", bool.FalseString);
        //public static int NumberOfQuestions;
    }
}
