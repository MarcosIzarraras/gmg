using GMG.Application.Feactures.Interfaces;
using GMG.Domain.Products.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Products.Queries.GetProductTypeById
{
    public class GetProductTypeByIdHandler(IProductTypeRepository productTypeRepository) : IRequestHandler<GetProductTypeByIdQuery, ProductType?>
    {
        public async Task<ProductType?> Handle(GetProductTypeByIdQuery request, CancellationToken cancellationToken)
        {
            return await productTypeRepository.GetByIdAsync(request.Id);
        }
    }
}
