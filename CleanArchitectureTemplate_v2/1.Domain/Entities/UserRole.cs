using _1.Domain.Commons;

namespace _1.Domain.Entities
{
    public class UserRole : BaseAuditableEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}