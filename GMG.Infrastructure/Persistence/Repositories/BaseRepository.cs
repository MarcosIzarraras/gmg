using GMG.Application.Common.Paginations;
using GMG.Application.Common.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GMG.Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private DbSet<T> _dbSet;
        private readonly AppDbContext _db;
        public BaseRepository(AppDbContext appDbContext)
        {
            _dbSet = appDbContext.Set<T>();
            _db = appDbContext;
        }

        public async Task<T?> GetByIdAsync(Guid id)
            => await _dbSet.FindAsync(id);

        public async Task<List<T>> ListAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<PaginationResult<T>> ListPaginatedAsync(
            Pagination pagination, 
            Expression<Func<T, bool>>? filter = null, 
            Expression<Func<T, object>>? orderBy = null, 
            bool descending = false,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);
            

            if (filter is not null)
                query = query.Where(filter);

            var total = await query.CountAsync();

            if (orderBy is not null)
                query = descending
                    ? query.OrderByDescending(orderBy)
                    : query.OrderBy(orderBy);

            var items = await query
                .Skip(pagination.Skip)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginationResult<T>
            {
                Items = items,
                TotalCount = total,
                Page = pagination.Page,
                PageSize = pagination.PageSize
            };
        }

        public async Task<List<T>> AddRangeAsync(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }
    }
}
