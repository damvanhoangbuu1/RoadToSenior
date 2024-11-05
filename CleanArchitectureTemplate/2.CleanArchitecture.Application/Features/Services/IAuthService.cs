using _1.CleanArchitecture.Domain.Common;
using _2.CleanArchitecture.Application.DTOs.Auth;

namespace _2.CleanArchitecture.Application.Features.Services
{
    public interface IAuthService
    {
        public Task<Result<string>> Login(string username, string password);

        public Task<Result<UserInfor>> UpdateInfor(UpdateInforRequest request);
    }
}