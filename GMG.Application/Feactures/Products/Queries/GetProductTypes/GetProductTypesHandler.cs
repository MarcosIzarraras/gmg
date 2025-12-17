using GMG.Application.Common.Persistence.Repositories;
using GMG.Domain.Products.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Products.Queries.GetProductTypes
{
    public class GetProductTypesHandler(IProductTypeRepository productTypeRepository) : IRequestHandler<GetProductTypesQuery, List<ProductType>>
    {
        public Task<List<ProductType>> Handle(GetProductTypesQuery request, CancellationToken cancellationToken)
            => productTypeRepository.ListAsync();
    }
}
