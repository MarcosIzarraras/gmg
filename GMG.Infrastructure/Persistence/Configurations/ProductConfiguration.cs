using GMG.Domain.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GMG.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);
            
            builder.Property(p => p.Description)
                .HasMaxLength(500);
            
            builder.Property(p => p.Price)
                .IsRequired();

            builder.Property(p => p.Stock)
                .IsRequired();
            
            // Relationships
            builder.HasOne(p => p.ProductType)
                .WithMany()
                .HasForeignKey(p => p.ProductTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.ProductImages)
                .WithOne(p => p.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
