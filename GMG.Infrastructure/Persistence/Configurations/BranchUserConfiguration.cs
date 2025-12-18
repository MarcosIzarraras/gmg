using GMG.Domain.Branches.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GMG.Infrastructure.Persistence.Configurations
{
    public class BranchUserConfiguration : IEntityTypeConfiguration<BranchUser>
    {
        public void Configure(EntityTypeBuilder<BranchUser> builder)
        {
            //KEY
            builder.HasKey(bu => bu.Id);

            //PROPERTIES
            builder.Property(bu => bu.Username)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(bu => bu.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(bu => bu.PasswordHash)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(bu => bu.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(bu => bu.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(bu => bu.Role)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
