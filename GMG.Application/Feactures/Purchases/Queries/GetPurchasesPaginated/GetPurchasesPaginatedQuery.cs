using GMG.Application.Common.Pagination;
using GMG.Domain.Purchases.Entities;
using MediatR;

namespace GMG.Application.Feactures.Purchases.Queries.GetPurchasesPaginated
{
    public record GetPurchasesPaginatedQuery(Pagination Pagination) : IRequest<PaginationResult<Purchase>>;
}
