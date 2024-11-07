using _1.Domain.Enums;

namespace _2.Application.DTOs.User
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public List<RoleType> Roles { get; set; } = new();
    }
}