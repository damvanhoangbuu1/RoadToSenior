using _1.CleanArchitecture.Domain.Entities;

namespace _2.CleanArchitecture.Application.Features.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsername(string username);
    }
}
