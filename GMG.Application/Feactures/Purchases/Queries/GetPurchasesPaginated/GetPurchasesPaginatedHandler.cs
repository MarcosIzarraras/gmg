using GMG.Application.Common.Paginations;
using GMG.Application.Common.Persistence.Repositories;
using GMG.Domain.Purchases.Entities;
using MediatR;

namespace GMG.Application.Feactures.Purchases.Queries.GetPurchasesPaginated
{
    public class GetPurchasesPaginatedHandler(IRepository<Purchase> repositoryPurchase) : IRequestHandler<GetPurchasesPaginatedQuery, PaginationResult<Purchase>>
    {
        public Task<PaginationResult<Purchase>> Handle(GetPurchasesPaginatedQuery request, CancellationToken cancellationToken)
        {
            var paginated = repositoryPurchase.ListPaginatedAsync(
                request.Pagination,
                x => string.IsNullOrEmpty(request.Pagination.Search)
                    || x.Total.ToString().Contains(request.Pagination.Search),
                x => x.CreatedAt,
                false,
                x => x.Details);

            return paginated;
        }
    }
}
