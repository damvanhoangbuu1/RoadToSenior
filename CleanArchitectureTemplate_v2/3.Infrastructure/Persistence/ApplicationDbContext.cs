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

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId == Guid.Empty ? "system" : _currentUserService.UserId.ToString();
                        entry.Entity.Created = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId == Guid.Empty ? "system" : _currentUserService.UserId.ToString();
                        entry.Entity.LastModified = DateTime.Now;
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            OnBeforeSaving();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            OnBeforeSaving();

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                                .HasKey("Id");

                    modelBuilder.Entity(entityType.ClrType)
                                .Property("Id")
                                .HasColumnName("id");
                }

                if (typeof(BaseAuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                                .HasKey("Id");

                    modelBuilder.Entity(entityType.ClrType)
                                .Property("Id")
                                .HasColumnName("id");

                    modelBuilder.Entity(entityType.ClrType)
                                .Property("Created")
                                .HasColumnName("created")
                                .HasColumnType("timestamp without time zone")
                                .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    modelBuilder.Entity(entityType.ClrType)
                                .Property("LastModified")
                                .HasColumnName("modified")
                                .HasColumnType("timestamp without time zone");

                    modelBuilder.Entity(entityType.ClrType)
                                .Property("CreatedBy")
                                .HasColumnName("created_by")
                                .HasColumnType("character varying");

                    modelBuilder.Entity(entityType.ClrType)
                                .Property("LastModifiedBy")
                                .HasColumnName("modified_by")
                                .HasColumnType("character varying");
                }
            }
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
                        RoleName = roleEnum.ToString(),
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

                await SaveChangesAsync();
            }
        }
    }
}