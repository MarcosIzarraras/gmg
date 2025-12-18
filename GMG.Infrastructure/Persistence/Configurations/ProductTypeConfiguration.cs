using GMG.Domain.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GMG.Infrastructure.Persistence.Configurations
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.HasKey(pt => pt.Id);
            builder.Property(pt => pt.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany<Product>()
                .WithOne(p => p.ProductType)
                .HasForeignKey(p => p.ProductTypeId);
        }
    }
}
