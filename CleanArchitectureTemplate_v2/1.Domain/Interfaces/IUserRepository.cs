using _1.Domain.Entities;
using _1.Domain.Interfaces.Commons;

namespace _1.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsername(string username);
    }
}
