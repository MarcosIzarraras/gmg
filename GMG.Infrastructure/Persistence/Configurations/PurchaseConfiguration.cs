using GMG.Domain.Purchases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GMG.Infrastructure.Persistence.Configurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.PurchaseDate)
                .IsRequired();

            builder.Property(p => p.SupplierId)
                .IsRequired();

            //INDEXES
            builder.HasIndex(p => p.PurchaseDate);
            builder.HasIndex(p => p.SupplierId);

            builder.OwnsMany(p => p.Details, pd =>
            {
                pd.HasKey(d => d.Id);
                
                pd.WithOwner()
                    .HasForeignKey(pd => pd.PurchaseId);
                
                pd.Property(d => d.ProductId)
                    .IsRequired();
                
                pd.Property(d => d.Quantity)
                    .IsRequired();
                
                pd.Property(d => d.UnitPrice)
                    .IsRequired();

                pd.Property(d => d.Total)
                    .IsRequired();

                pd.HasIndex(p => p.ProductId);
            });
        }
    }
}
