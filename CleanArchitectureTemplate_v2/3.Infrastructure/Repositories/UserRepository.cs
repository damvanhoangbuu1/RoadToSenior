using _1.Domain.Entities;
using _1.Domain.Interfaces;
using _3.Infrastructure.Persistence;
using _3.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace _3.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        protected readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByUsername(string username) => await _context.Users.Include(p => p.UserRoles).ThenInclude(p => p.Role).FirstOrDefaultAsync();
    }
}
