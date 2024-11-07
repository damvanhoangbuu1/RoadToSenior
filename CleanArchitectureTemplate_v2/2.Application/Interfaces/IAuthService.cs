using _2.Application.Common;
using _2.Application.DTOs.User;

namespace _2.Application.Interfaces
{
    public interface IAuthService
    {
        Result<string> Login(string username, string password);
        Result<UserDto> GetAccountInfor();
        Task<Result<UserDto>> UpdateAccountInfor(UserDto userDto);
    }
}
