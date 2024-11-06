using _1.Domain.Commons;
using _1.Domain.Enums;

namespace _1.Domain.Entities
{
    public class Role : IEntity
    {
        public Guid Id { get; set; }
        public RoleType RoleType { get; set; }
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}