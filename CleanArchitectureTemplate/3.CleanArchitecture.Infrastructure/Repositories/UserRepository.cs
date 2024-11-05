using _1.CleanArchitecture.Domain.Entities;
using _2.CleanArchitecture.Application.Features.IRepositories;
using _2.CleanArchitecture.Application.Features.Repositories.Commons;
using _3.CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace _3.CleanArchitecture.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        protected readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByUsername(string username) => await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
    }
}