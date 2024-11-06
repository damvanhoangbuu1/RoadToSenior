using _1.Domain.Entities;

namespace _1.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUsername(string username);
    }
}
