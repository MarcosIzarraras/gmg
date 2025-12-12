using GMG.Domain.Common;
using GMG.Domain.Products.Entities;
using GMG.Domain.Purchases.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductType> ProductTypes => Set<ProductType>();
        public DbSet<Purchase> Purchases => Set<Purchase>();
        public DbSet<PurchaseDetail> PurchaseDetails => Set<PurchaseDetail>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetBaseFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetBaseFields()
        {
            var date = DateTime.Now;
            var user = "System";
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            baseEntity.CreatedBy = user;
                            baseEntity.CreatedAt = date;
                            break;
                        case EntityState.Modified:
                            baseEntity.UpdatedBy = user;
                            baseEntity.UpdatedAt = date;
                            break;
                    }
                }
            }
        }
    }
}
