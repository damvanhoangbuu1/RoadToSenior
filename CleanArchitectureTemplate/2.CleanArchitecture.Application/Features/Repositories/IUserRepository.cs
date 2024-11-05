using _1.CleanArchitecture.Domain.Entities;
using _2.CleanArchitecture.Application.Common.Interfaces;

namespace _2.CleanArchitecture.Application.Features.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsername(string username);
    }
}