namespace _2.CleanArchitecture.Application.DTOs.Auth
{
    public class UpdateInforRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}