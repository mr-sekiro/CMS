namespace CMS.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string Subject, string Body);
    }
}
