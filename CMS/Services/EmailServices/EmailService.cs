using System.Net;
using System.Net.Mail;

namespace CMS.Services.EmailServices
{
    public class EmailService(IConfiguration _configuration) : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var from = _configuration["EmailSettings:From"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]!);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            var message = new MailMessage(from!, toEmail, subject, body);
            message.IsBodyHtml = true;

            using var client = new SmtpClient(smtpServer, port) 
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            }; 

            await client.SendMailAsync(message);
        }
    }
}
