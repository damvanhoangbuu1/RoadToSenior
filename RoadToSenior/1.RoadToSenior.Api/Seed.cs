using _1.RoadToSenior.Api.Models.Auth;

namespace _1.RoadToSenior.Api
{
    public class Seed
    {
        public static List<User> Users { get; } = new List<User>
        {
            new User { Username = "admin", Password = "admin123", Role = Role.Admin, Permission = Permission.Edit },
            new User { Username = "user1", Password = "user123", Role = Role.Admin, Permission = Permission.Wiew },
            new User { Username = "user2", Password = "user123", Role = Role.User, Permission = Permission.Wiew },
            new User { Username = "user3", Password = "user123", Role = Role.User, Permission = Permission.Wiew },
            new User { Username = "user4", Password = "user123", Role = Role.User, Permission = Permission.Wiew },
            new User { Username = "user5", Password = "user123", Role = Role.User, Permission = Permission.Wiew },
            new User { Username = "user6", Password = "user123", Role = Role.User, Permission = Permission.Edit },
        };
    }
}
