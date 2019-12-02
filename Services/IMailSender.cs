using QuestionnaireApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnaireApp.Services
{
    public interface IMailSender
    {
        Task Send(Message message);
    }
}
