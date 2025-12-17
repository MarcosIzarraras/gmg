using GMG.Application.Common.Paginations;
using GMG.Application.Common.Persistence.Repositories;
using GMG.Domain.Products.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GMG.Infrastructure.Persistence.Repositories
{
    public class ProductTypeRepository : BaseRepository<ProductType>, IProductTypeRepository
    {
        private DbSet<ProductType> _dbSet;
        public ProductTypeRepository(AppDbContext db) : base(db)
        {
            _dbSet = db.Set<ProductType>();
        }

        public Task<bool> ExistsByNameAsync(string name)
            => _dbSet.AnyAsync(x => x.Name == name);
    }
}
