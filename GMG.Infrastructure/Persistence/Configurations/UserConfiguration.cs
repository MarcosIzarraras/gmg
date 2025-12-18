using GMG.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GMG.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.PasswordHash)
                .HasMaxLength(1000)
                .IsRequired();
            builder.Property(u => u.FirstName)
                .IsRequired();
            builder.Property(u => u.LastName)
                .IsRequired();
            builder.Property(u => u.UserRole)
                .IsRequired();
            builder.Property(u => u.IsActive)
                .IsRequired();
            builder.Property(u => u.CreatedAt)
                .IsRequired();
            builder.Property(u => u.UpdateAt).IsRequired();
            builder.Property(u => u.LastLoginAt);

            builder.HasMany(u => u.Branches)
                .WithOne(b => b.Owner)
                .HasForeignKey(b => b.OwnerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
