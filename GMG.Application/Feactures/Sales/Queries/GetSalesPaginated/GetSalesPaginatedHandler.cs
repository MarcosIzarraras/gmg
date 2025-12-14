using GMG.Application.Common.Pagination;
using GMG.Application.Feactures.Interfaces;
using GMG.Domain.Sales.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Sales.Queries.GetSalesPaginated
{
    public class GetSalesPaginatedHandler(IRepository<Sale> saleRepository) : IRequestHandler<GetSalesPaginatedQuery, PaginationResult<SalesPaginatedDto>>
    {
        public async Task<PaginationResult<SalesPaginatedDto>> Handle(GetSalesPaginatedQuery request, CancellationToken cancellationToken)
        {
            var paginated = await saleRepository.ListPaginatedAsync(
                request.Pagination, 
                filter => (string.IsNullOrEmpty(request.Pagination.Search) || (filter.Customer != null && filter.Customer.Name.Contains(request.Pagination.Search))),
                order => order.CreatedAt,
                true,
                inlcude => inlcude.Customer!
                );

            var dtos = paginated.Items.Select(s => new SalesPaginatedDto
            {
                Id = s.Id,
                Client = s.Customer?.Name ?? "",
                CreatedAt = s.CreatedAt.ToString("dd MMM yyyy HH:mm"),
                Total = s.Total.ToString("C")
            }).ToList();

            return new PaginationResult<SalesPaginatedDto>()
            {
                Items = dtos,
                Page = paginated.Page,
                PageSize = paginated.PageSize,
                TotalCount = paginated.TotalCount
            };
        }
    }
}
