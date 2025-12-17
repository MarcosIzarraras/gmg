using GMG.Domain.Branches.Entities;

namespace GMG.Application.Common.Persistence.Repositories
{
    public interface IBranchRepository : IRepository<Branch>
    {
        Task<Branch?> GetMainBranch(Guid ownerId);
    }
}
