using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using QuestionnaireApp.Models;

namespace QuestionnaireApp.Services
{
    public class SmtpMailSender : IMailSender
    {
        public async Task Send(Message message)
        {
            using(var smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("konopie2019@gmail.com","Qwer!234");
                var msg = new MailMessage
                {
                    IsBodyHtml = true,
                    Body = message.Body,
                    Subject = message.Subject,
                    From = new MailAddress("konopie2019@gmail.com", "QuestionnairesSupport")
                };
                for (int i = 0; i < message.To.Length; i++)
                    msg.To.Add(message.To[i]);
                await smtp.SendMailAsync(msg);
            }
        }
    }
}
