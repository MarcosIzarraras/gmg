using GMG.Domain.Products.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Common.Persistence.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<bool> ExistAsync(string name);
        public Task<Product?> GetProductWithImages(Guid productId);
    }
}
