using _1.CleanArchitecture.Domain.Common;

namespace _1.CleanArchitecture.Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
