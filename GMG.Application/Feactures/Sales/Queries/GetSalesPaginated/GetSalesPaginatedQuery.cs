using GMG.Application.Common.Paginations;
using MediatR;

namespace GMG.Application.Feactures.Sales.Queries.GetSalesPaginated
{
    public record GetSalesPaginatedQuery(Pagination Pagination) : IRequest<PaginationResult<SalesPaginatedDto>>;
}
