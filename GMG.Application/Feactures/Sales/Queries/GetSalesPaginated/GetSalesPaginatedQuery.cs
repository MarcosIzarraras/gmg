using GMG.Application.Common.Pagination;
using MediatR;

namespace GMG.Application.Feactures.Sales.Queries.GetSalesPaginated
{
    public record GetSalesPaginatedQuery(Pagination Pagination) : IRequest<PaginationResult<SalesPaginatedDto>>;
}
