using GMG.Domain.Branches.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GMG.Infrastructure.Persistence.Configurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasKey(b => b.Id);

            //PROPERTIES
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.Address)
                .HasMaxLength(500);

            builder.Property(b => b.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(b => b.Email)
                .HasMaxLength(100);

            //INDEXES
            builder.HasIndex(b => b.Name);

            builder.HasMany(i => i.BranchUsers)
                .WithOne(i => i.Branch)
                .HasForeignKey(bu => bu.BranchId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
