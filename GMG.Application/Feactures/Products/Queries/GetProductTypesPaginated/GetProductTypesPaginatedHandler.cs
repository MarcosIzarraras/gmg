using GMG.Application.Common.Pagination;
using GMG.Application.Feactures.Interfaces;
using GMG.Domain.Common.Result;
using GMG.Domain.Products.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Products.Queries.GetProductTypesPaginated
{
    public class GetProductTypesPaginatedHandler : IRequestHandler<GetProductTypesPaginatedQuery, PaginationResult<ProductType>>
    {
        private readonly IProductTypeRepository _productTypeRepository;
        public GetProductTypesPaginatedHandler(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }   
        public async Task<PaginationResult<ProductType>> Handle(GetProductTypesPaginatedQuery request, CancellationToken cancellationToken)
        {
            return await _productTypeRepository.ListPaginatedAsync(
                request.Pagination,
                x => (string.IsNullOrEmpty(request.Pagination.Search) || x.Name == request.Pagination.Search), 
                x => x.Name);
        }
    }
}
