using GMG.Domain.Products.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<bool> ExistAsync(string name);
    }
}
