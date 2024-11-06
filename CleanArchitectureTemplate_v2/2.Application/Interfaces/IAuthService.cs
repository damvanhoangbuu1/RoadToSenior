using _2.Application.Common;

namespace _2.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<string>> Login(string username, string password);
    }
}
