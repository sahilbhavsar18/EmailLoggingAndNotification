using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        public async Task SendAsync(string to, string subject, string body)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("bhavsarsahil48@gmail.com", "byrx leoe yofq miqw"),
                EnableSsl = true
            };
            var email = new MailMessage()
            {
                From = new MailAddress("bhavsarsahil48@gmail.com","EmailSender"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            email.To.Add(to);
            await client.SendMailAsync(email);
        }
    }
}
