using _3.RoadToSenior.Identiy.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _3.RoadToSenior.Identiy.Data
{
    public class ManageAppDBContext: IdentityDbContext<ManageUser>
    {
        public ManageAppDBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>()
                .Property(x => x.Id).HasMaxLength(50)
                .IsRequired();

            builder.Entity<ManageUser>()
                .Property(x => x.Id)
                .HasMaxLength(50)
                .IsRequired();

            builder.Entity<ManageUser>()
                .Property(x => x.DisplayName)
                .IsRequired(false);
        }

        public DbSet<ManageUser> ManageUsers { get; set; }
    }
}
