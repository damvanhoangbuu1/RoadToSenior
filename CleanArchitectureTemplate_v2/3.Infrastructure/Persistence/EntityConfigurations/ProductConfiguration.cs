using _1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3.Infrastructure.Persistence.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.HasMany(p => p.Items)
                   .WithOne(c => c.Product)
                   .HasForeignKey(p => p.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(u => u.ProductName)
                   .HasMaxLength(20)
                   .IsRequired()
                   .HasColumnName("product_name")
                   .HasColumnType("character varying");

            builder.Property(u => u.ProductDescription)
                   .IsRequired(false)
                   .HasColumnName("product_description")
                   .HasColumnType("text");

            builder.Property(u => u.ProductPrice)
                   .IsRequired()
                   .HasColumnName("product_price")
                   .HasColumnType("decimal(9,0)");

            builder.Property(u => u.ProductDiscount)
                   .IsRequired()
                   .HasColumnName("product_discount")
                   .HasColumnType("decimal(2,0)")
                   .HasDefaultValue(0);
        }
    }
}