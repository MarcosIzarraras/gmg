using GMG.Application.Common.Persistence;
using GMG.Application.Common.Persistence.Repositories;
using GMG.Infrastructure.Persistence.Repositories;

namespace GMG.Infrastructure.Persistence
{
    public class UnitOfWork(AppDbContext _db) : IUnitOfWork
    {
        private IProductRepository? _products;
        private IBranchRepository? _branchs;
        private IBranchUserRepository? _branchUsers;
        private IProductTypeRepository? _productTypes;
        private IUserRepository? _users;
        private bool _disposed = false;


        public IProductRepository Products 
            => _products ?? new ProductRepository(_db);

        public IBranchRepository Branches 
            => _branchs ?? new BranchRepository(_db);

        public IBranchUserRepository BranchUsers
            => _branchUsers ?? new BranchUserRepository(_db);

        public IProductTypeRepository ProductTypes 
            => _productTypes ?? new ProductTypeRepository(_db);

        public IUserRepository UserRepository 
            => _users ?? new UserRepository(_db);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _db.Dispose();
            }
            _disposed = true;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
            => _db.SaveChangesAsync(cancellationToken);

        public void SetTenantContext(Guid ownerId, Guid? branchId, Guid? branchUserId, bool isOwner)
        {
            _db.SetTenantContext(ownerId, branchId, branchUserId, isOwner);
        }
    }
}
