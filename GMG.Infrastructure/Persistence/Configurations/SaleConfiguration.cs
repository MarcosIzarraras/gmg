
using GMG.Domain.Sales.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GMG.Infrastructure.Persistence.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Total)
                .IsRequired();

            builder.Property(s => s.IsCancelled)
                .IsRequired();

            builder.Property(s => s.CancelledAt);

            builder.HasOne(s => s.Customer)
                .WithMany()
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(s => s.CustomerId);

            builder.OwnsMany(s => s.Details, sd =>
            {
                sd.HasKey("Id");

                sd.WithOwner()
                    .HasForeignKey(sd => sd.SaleId);

                sd.Property(sd => sd.ProductId)
                    .IsRequired();

                sd.Property(sd => sd.Quantity)
                    .IsRequired();

                sd.Property(sd => sd.UnitPrice)
                    .IsRequired();

                sd.Property(sd => sd.Total)
                    .IsRequired();

                sd.HasIndex(sd => sd.ProductId);
            });
        }
    }
}
