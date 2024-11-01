using _1.CleanArchitecture.Domain.Entities;

namespace _2.CleanArchitecture.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
