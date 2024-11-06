using _1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3.Infrastructure.Persistence.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Username)
                .HasColumnType("character varying(20)")
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnType("character varying(20)")
                .IsRequired(false);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

        }
    }
}