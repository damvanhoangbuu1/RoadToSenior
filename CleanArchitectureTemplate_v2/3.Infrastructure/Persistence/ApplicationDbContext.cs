using _1.Domain.Commons;
using _1.Domain.Entities;
using _1.Domain.Interfaces.Commons;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace _3.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

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
                        CreatedBy = "system",
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "john_doe",
                        Email = "johndoe@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password@123"),
                        CreatedBy = "system",
                    }
                };

                await Users.AddRangeAsync(initialUsers);
                await SaveChangesAsync();
            }
        }
    }
}