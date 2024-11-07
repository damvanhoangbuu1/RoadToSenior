using _2.Application.DTOs.User;

namespace _2.Application.Interfaces
{
    public interface IUserService
    {
        UserDto GetUserInfor(Guid Id);
    }
}