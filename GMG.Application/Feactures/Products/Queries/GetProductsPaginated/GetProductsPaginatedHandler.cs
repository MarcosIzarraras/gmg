using GMG.Application.Common.Paginations;
using GMG.Application.Common.Persistence.Repositories;
using GMG.Domain.Products.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Products.Queries.GetProductsPaginated
{
    public class GetProductsPaginatedHandler(IProductRepository productRepository) : IRequestHandler<GetProductsPaginatedQuery, PaginationResult<Product>>
    {
        public Task<PaginationResult<Product>> Handle(GetProductsPaginatedQuery request, CancellationToken cancellationToken)
            => productRepository.ListPaginatedAsync(
                request.Pagination,
                x => string.IsNullOrEmpty(request.Pagination.Search) ||
                     x.Name.Contains(request.Pagination.Search) ||
                     x.Description.Contains(request.Pagination.Search),
                x => x.Name,
                false,
                x => x.ProductType);
    }
}
