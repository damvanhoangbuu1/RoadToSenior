using _1.Domain.Entities;

namespace _1.Domain.Interfaces.Commons
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(User user);
    }
}
