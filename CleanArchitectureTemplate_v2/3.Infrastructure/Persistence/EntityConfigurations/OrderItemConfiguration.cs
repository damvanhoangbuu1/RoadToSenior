using _1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3.Infrastructure.Persistence.EntityConfigurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasOne(p => p.Order)
                   .WithMany(c => c.Items)
                   .HasForeignKey(c => c.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Product)
                   .WithMany(c => c.Items)
                   .HasForeignKey(c => c.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(u => u.Quantity)
                   .IsRequired();
        }
    }
}