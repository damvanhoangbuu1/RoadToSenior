using _1.CleanArchitecture.Domain.Common;

namespace _2.CleanArchitecture.Application.DTOs.Auth
{
    public class UserInfor : BaseAuditableEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}