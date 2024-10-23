namespace _1.RoadToSenior.Api.Models.Auth
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }

    public enum Role
    {
        Admin,
        User
    }

    public enum Permission
    {
        Edit,
        Wiew
    }
}
