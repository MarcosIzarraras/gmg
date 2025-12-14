using GMG.Application.Common.Pagination;
using GMG.Domain.Common;
using System.Linq.Expressions;

namespace GMG.Application.Feactures.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<List<T>> ListAsync(params Expression<Func<T, object>>[] includes);
        Task<T> AddAsync(T entity);
        Task<List<T>> AddRangeAsync(List<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<PaginationResult<T>> ListPaginatedAsync(
            Pagination pagination, 
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>? orderBy = null,
            bool descending = false,
            params Expression<Func<T, object>>[] includes);
    }
}
