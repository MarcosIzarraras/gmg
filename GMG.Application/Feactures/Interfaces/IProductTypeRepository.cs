using GMG.Domain.Products.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Interfaces
{
    public interface IProductTypeRepository : IRepository<ProductType>
    {
        Task<bool> ExistsByNameAsync(string name);
    }
}
