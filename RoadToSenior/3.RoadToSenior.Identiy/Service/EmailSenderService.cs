using Microsoft.AspNetCore.Identity.UI.Services;

namespace _3.RoadToSenior.Identiy.Service
{
    public class EmailSenderService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
        }
    }
}
