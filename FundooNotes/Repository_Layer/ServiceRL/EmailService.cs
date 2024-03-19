using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Layer.ServiceRL
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {



            // Initialize SmtpClient with SMTP server settings
            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("example@gmail.com", "your_password"),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mailMessage = new MailMessage("example@gmail.com", to, subject, body)
            {
                IsBodyHtml = false
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }

    }
}

