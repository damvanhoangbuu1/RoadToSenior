using _1.Domain.Entities;
using _1.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3.Infrastructure.Persistence.EntityConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasMany(p => p.Items)
                   .WithOne(c => c.Order)
                   .HasForeignKey(p => p.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(u => u.TotalMoney)
                   .IsRequired()
                   .HasColumnType("numeric(9, 0)");

            builder.Property(u => u.Status)
                   .IsRequired()
                   .HasDefaultValue(OrderStatus.New)
                   .HasConversion<int>()
                   .HasColumnType("numeric(1, 0)");
        }
    }
}