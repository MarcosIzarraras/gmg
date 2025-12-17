using GMG.Domain.Branches.Entities;

namespace GMG.Application.Common.Persistence.Repositories
{
    public interface IBranchUserRepository
    {
        Task<BranchUser?> GetByEmailAsync(string email);
    }
}
