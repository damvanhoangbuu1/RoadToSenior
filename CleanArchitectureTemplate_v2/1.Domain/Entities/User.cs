using _1.Domain.Commons;
using _1.Domain.Enums;

namespace _1.Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<UserRole> Roles { get; set; } = new();
    }
}