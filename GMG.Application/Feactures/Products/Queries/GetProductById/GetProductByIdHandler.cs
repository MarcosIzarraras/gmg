using GMG.Application.Common.Persistence.Repositories;
using GMG.Domain.Products.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Products.Queries.GetProductById
{
    public class GetProductByIdHandler(IProductRepository productRepository) : IRequestHandler<GetProductByIdQuery, Product?>
    {
        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            Product? product = null;
            product = await productRepository.GetByIdAsync(request.Id);
            return product;
        }
    }
}
