namespace _2.CleanArchitecture.Application.Helper
{
    public static class CryptHelper
    {
        public static bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}