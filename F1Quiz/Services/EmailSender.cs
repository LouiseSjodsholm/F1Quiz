using Microsoft.AspNetCore.Identity.UI.Services;

namespace F1Quiz.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //testing purposes
            Console.WriteLine($"Sending email to {email}: {subject} - {htmlMessage}");
            return Task.CompletedTask;
        }

    }
}
