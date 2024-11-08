using _1.Domain.Commons;
using _1.Domain.Enums;

namespace _1.Domain.Entities
{
    public class Role : BaseEntity
    {
        public RoleType RoleType { get; set; }
        public string RoleName { get; set; }
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}