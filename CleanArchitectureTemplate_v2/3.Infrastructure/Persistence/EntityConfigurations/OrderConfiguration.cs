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
            builder.ToTable("orders");

            builder.HasMany(p => p.Items)
                   .WithOne(c => c.Order)
                   .HasForeignKey(p => p.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(u => u.TotalMoney)
                   .IsRequired()
                   .HasColumnType("numeric(9, 0)")
                   .HasColumnName("total_money");

            builder.Property(u => u.Status)
                   .IsRequired()
                   .HasDefaultValue(OrderStatus.New)
                   .HasConversion<int>()
                   .HasColumnType("numeric(1, 0)")
                   .HasColumnName("status");

            builder.HasOne(p => p.User)
                   .WithMany(p => p.Orders)
                   .HasForeignKey(p => p.UserId);

            builder.Property(p => p.UserId)
                   .HasColumnName("user_id");
        }
    }
}