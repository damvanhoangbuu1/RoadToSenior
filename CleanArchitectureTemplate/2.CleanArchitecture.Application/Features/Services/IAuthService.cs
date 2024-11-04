using _1.CleanArchitecture.Domain.Common;

namespace _2.CleanArchitecture.Application.Features.IServices
{
    public interface IAuthService
    {
        public Task<Result<string>> Login(string username, string password);
    }
}
