using GMG.Application.Feactures.Interfaces;

namespace GMG.Infrastructure.Persistence
{
    public class UnitOfWork(AppDbContext _db) : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
            => _db.SaveChangesAsync(cancellationToken);
    }
}
