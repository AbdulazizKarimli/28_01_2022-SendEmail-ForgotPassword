using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FrontToBack.Utils
{
    public static class EmailUtil
    {
        public async static Task SendEmailAsync(string email, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress(Constants.EmailAddress);
            mail.Subject = subject;
            string Body = body;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(Constants.EmailAddress, Constants.Password); // Enter seders User name and password  
            smtp.EnableSsl = true;
            try
            {
               await smtp.SendMailAsync(mail);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
