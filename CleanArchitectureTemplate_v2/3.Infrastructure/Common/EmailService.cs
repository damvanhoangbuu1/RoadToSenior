using _1.Domain.Interfaces;

namespace _3.Infrastructure.Common
{
    public class EmailService : IEmailService
    {
        public void VerifyEmail(string email)
        {
            Console.WriteLine(email);
        }
    }
}
