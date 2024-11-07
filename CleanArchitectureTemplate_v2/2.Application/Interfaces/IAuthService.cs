using _2.Application.Common;
using _2.Application.DTOs;

namespace _2.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<string>> Login(string username, string password);
        Result<UserDto> GetAccountInfor();
    }
}
