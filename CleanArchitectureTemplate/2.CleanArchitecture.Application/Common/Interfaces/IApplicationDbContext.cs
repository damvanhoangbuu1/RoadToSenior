using _1.CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _2.CleanArchitecture.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
