using _2.Application.DTOs;

namespace _2.Application.Interfaces
{
    public interface IUserService
    {
        UserDto GetUserInfor(Guid Id);
    }
}