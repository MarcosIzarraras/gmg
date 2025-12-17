using GMG.Application.Common.Persistence.Repositories;

namespace GMG.Application.Common.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IBranchRepository Branches { get; }
        IBranchUserRepository BranchUsers { get; }
        IProductTypeRepository ProductTypes { get; }
        IUserRepository UserRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void SetTenantContext(Guid ownerId, Guid? branchId, Guid? branchUserId, bool isOwner);
    }
}
