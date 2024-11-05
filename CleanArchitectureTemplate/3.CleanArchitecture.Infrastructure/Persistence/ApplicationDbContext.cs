using _1.CleanArchitecture.Domain.Common;
using _1.CleanArchitecture.Domain.Entities;
using _2.CleanArchitecture.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace _3.CleanArchitecture.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(
            DbContextOptions options,
            ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<User> Users => Set<User>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId.ToString();
                        entry.Entity.Created = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId.ToString();
                        entry.Entity.LastModified = DateTime.UtcNow;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public async Task Initialize()
        {
            if (!Users.Any())
            {
                var initialUsers = new List<User>
                {
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "admin",
                        Email = "admin@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                        Roles = new List<string> { "Admin", "User" },
                        CreatedBy = "system",
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "john_doe",
                        Email = "johndoe@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password@123"),
                        Roles = new List<string> { "User" },
                        CreatedBy = "system",
                    }
                };

                await Users.AddRangeAsync(initialUsers);
                await SaveChangesAsync();
            }
        }
    }
}