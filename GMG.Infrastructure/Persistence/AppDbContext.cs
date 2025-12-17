using GMG.Domain.Branches.Entities;
using GMG.Domain.Common;
using GMG.Domain.Customers.Entities;
using GMG.Domain.Products.Entities;
using GMG.Domain.Purchases.Entities;
using GMG.Domain.Sales.Entities;
using GMG.Domain.Suppliers.Entities;
using GMG.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GMG.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        #region DBSET
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<BranchUser> BranchUsers => Set<BranchUser>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductType> ProductTypes => Set<ProductType>();
        public DbSet<Purchase> Purchases => Set<Purchase>();
        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<User> Users => Set<User>();
        #endregion DBSET

        private Guid? _currentOwnerId;
        private Guid? _branchUserId;
        private Guid? _currentBranchId;
        private bool _tenantContextSet = false;
        private bool _isOwner;

        private Guid CurrentOwnerId => _currentOwnerId ?? Guid.Empty;
        private Guid AuditUserId => _branchUserId ?? CurrentOwnerId;
        private Guid? CurrentBranchId => _currentBranchId;
        private bool IsOwner => _isOwner;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public void SetTenantContext(Guid ownerId, Guid? branchId, Guid? branchUserId, bool isOwner)
        {
            _currentOwnerId = ownerId;
            _currentBranchId = branchId;
            _isOwner = isOwner;
            _tenantContextSet = true;
            _branchUserId = branchUserId;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                // Para OwnBranch
                if (typeof(OwnBranch).IsAssignableFrom(clrType) && !clrType.IsAbstract)
                {
                    if (IsOwner)
                    {
                        // Owner ve TODAS sus sucursales
                        var method = typeof(AppDbContext)
                            .GetMethod(nameof(SetQueryFilterForOwnBranchOwner), BindingFlags.NonPublic | BindingFlags.Instance)!
                            .MakeGenericMethod(clrType);
                        method.Invoke(this, new object[] { modelBuilder });
                    }
                    else
                    {
                        // Usuario normal ve solo su sucursal
                        var method = typeof(AppDbContext)
                            .GetMethod(nameof(SetQueryFilterForOwnBranch), BindingFlags.NonPublic | BindingFlags.Instance)!
                            .MakeGenericMethod(clrType);
                        method.Invoke(this, new object[] { modelBuilder });
                    }
                }
                // Para OwnMultipleBranches
                else if (typeof(OwnMultipleBranches).IsAssignableFrom(clrType) && !clrType.IsAbstract)
                {
                    if (IsOwner)
                    {
                        // Owner ve TODO de su organización
                        var method = typeof(AppDbContext)
                            .GetMethod(nameof(SetQueryFilterForOwnMultipleBranchesOwner), BindingFlags.NonPublic | BindingFlags.Instance)!
                            .MakeGenericMethod(clrType);
                        method.Invoke(this, new object[] { modelBuilder });
                    }
                    else
                    {
                        // Usuario normal ve solo su sucursal o registros sin sucursal
                        var method = typeof(AppDbContext)
                            .GetMethod(nameof(SetQueryFilterForOwnMultipleBranches), BindingFlags.NonPublic | BindingFlags.Instance)!
                            .MakeGenericMethod(clrType);
                        method.Invoke(this, new object[] { modelBuilder });
                    }
                }
                // Para OwnUser
                else if (typeof(OwnUser).IsAssignableFrom(clrType) && !clrType.IsAbstract)
                {
                    // Tanto Owner como BranchUser filtran solo por OwnerId
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(SetQueryFilterForOwnUser), BindingFlags.NonPublic | BindingFlags.Instance)!
                        .MakeGenericMethod(clrType);
                    method.Invoke(this, new object[] { modelBuilder });
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        // Filtros para BranchUser (existentes)
        private void SetQueryFilterForOwnBranch<T>(ModelBuilder modelBuilder) where T : OwnBranch
        {
            modelBuilder.Entity<T>().HasQueryFilter(e =>
                e.OwnerId == CurrentOwnerId && e.BranchId == CurrentBranchId);
        }

        private void SetQueryFilterForOwnMultipleBranches<T>(ModelBuilder modelBuilder) where T : OwnMultipleBranches
        {
            modelBuilder.Entity<T>().HasQueryFilter(e =>
                e.OwnerId == CurrentOwnerId && (e.BranchId == null || e.BranchId == CurrentBranchId));
        }

        private void SetQueryFilterForOwnUser<T>(ModelBuilder modelBuilder) where T : OwnUser
        {
            modelBuilder.Entity<T>().HasQueryFilter(e =>
                e.OwnerId == CurrentOwnerId);
        }

        // Nuevos filtros para Owner
        private void SetQueryFilterForOwnBranchOwner<T>(ModelBuilder modelBuilder) where T : OwnBranch
        {
            // Owner ve todas las sucursales
            modelBuilder.Entity<T>().HasQueryFilter(e =>
                e.OwnerId == CurrentOwnerId);
        }

        private void SetQueryFilterForOwnMultipleBranchesOwner<T>(ModelBuilder modelBuilder, Guid ownerId) where T : OwnMultipleBranches
        {
            // Owner ve todo de su organización
            modelBuilder.Entity<T>().HasQueryFilter(e =>
                e.OwnerId == ownerId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConvertDatesToUtc();
            SetTimestampsAndOwnerIds();
            return base.SaveChangesAsync(cancellationToken);
        }
        
        private void SetTimestampsAndOwnerIds()
        {
            var date = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is OwnBranch branchEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            branchEntity.CreatedBy = AuditUserId;
                            branchEntity.CreatedAt = date;
                            branchEntity.BranchId = CurrentBranchId!.Value;
                            branchEntity.OwnerId = CurrentOwnerId;
                            break;
                        case EntityState.Modified:
                            branchEntity.UpdatedBy = AuditUserId;
                            branchEntity.UpdatedAt = date;
                            break;
                    }
                }
                if (entry.Entity is OwnUser && entry.Entity is OwnMultipleBranches multipleBranchEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            multipleBranchEntity.CreatedBy = AuditUserId;
                            multipleBranchEntity.CreatedAt = date;
                            multipleBranchEntity.BranchId = CurrentBranchId!.Value;
                            multipleBranchEntity.OwnerId = CurrentOwnerId;
                            break;
                        case EntityState.Modified:
                            multipleBranchEntity.UpdatedBy = AuditUserId;
                            multipleBranchEntity.UpdatedAt = date;
                            break;
                    }
                }
            }
        }

        private void ConvertDatesToUtc()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                foreach (var property in entry.Properties)
                {
                    if (property.CurrentValue is DateTime dateTime && dateTime.Kind == DateTimeKind.Local)
                    {
                        property.CurrentValue = dateTime.ToUniversalTime();
                    }
                }
            }
        }
    }
}
