using _1.Domain.Commons;
using _1.Domain.Entities;
using _1.Domain.Enums;
using _1.Domain.Interfaces;
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
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
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
            if (!Roles.Any())
            {
                foreach (RoleType roleEnum in Enum.GetValues(typeof(RoleType)))
                {
                    await Roles.AddAsync(new Role
                    {
                        Id = Guid.NewGuid(),
                        RoleType = roleEnum,
                    });
                }

                await SaveChangesAsync();
            };

            if (!Users.Any())
            {
                var roleAdmin = Roles.FirstOrDefault(r => r.RoleType == RoleType.Admin);
                var roleUser = Roles.FirstOrDefault(r => r.RoleType == RoleType.User);
                var adminOnly = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    CreatedBy = "system",
                };

                Users.Add(adminOnly);

                UserRoles.Add(new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = adminOnly.Id,
                    RoleId = roleAdmin.Id,
                });

                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "admin1",
                    Email = "admin1@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    CreatedBy = "system",
                };

                Users.Add(admin);

                UserRoles.AddRange(new List<UserRole> {
                    new UserRole
                    {
                        Id = Guid.NewGuid(),
                        UserId = admin.Id,
                        RoleId = roleAdmin.Id,
                    },
                    new UserRole
                    {
                        Id = Guid.NewGuid(),
                        UserId = admin.Id,
                        RoleId = roleUser.Id,
                    }
                });

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "string",
                    Email = "string@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("string"),
                    CreatedBy = "system",
                };

                Users.Add(user);

                UserRoles.Add(new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    RoleId = roleUser.Id,
                });

                SaveChanges();
            }
        }
    }
}