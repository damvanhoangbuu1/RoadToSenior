namespace _3.Infrastructure.Services
{
    public static class PasswordHelper
    {
        public static bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
