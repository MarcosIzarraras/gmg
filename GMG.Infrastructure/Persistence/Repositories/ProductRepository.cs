using GMG.Application.Feactures.Interfaces;
using GMG.Domain.Products.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private DbSet<Product> _dbSet;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _dbSet = context.Set<Product>();
        }

        public Task<bool> ExistAsync(string name)
            => _dbSet.AnyAsync(p => p.Name == name);
    }
}
