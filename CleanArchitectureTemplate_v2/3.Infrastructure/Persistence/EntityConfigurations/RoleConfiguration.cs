using _1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3.Infrastructure.Persistence.EntityConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");

            builder.Property(r => r.RoleType)
                   .HasConversion<int>()
                   .HasColumnName("role_type")
                   .IsRequired()
                   .HasColumnType("decimal(1,0)");

            builder.Property(r => r.RoleName)
                   .HasColumnName("role_name")
                   .IsRequired()
                   .HasColumnType("character varying");

            builder.HasMany(p => p.UserRoles)
                   .WithOne(p => p.Role)
                   .HasForeignKey(p => p.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}