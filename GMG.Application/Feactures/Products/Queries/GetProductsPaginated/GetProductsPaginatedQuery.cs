using GMG.Application.Common.Pagination;
using GMG.Domain.Products.Entities;
using MediatR;

namespace GMG.Application.Feactures.Products.Queries.GetProductsPaginated
{
    public record GetProductsPaginatedQuery(Pagination Pagination) : IRequest<PaginationResult<Product>>;
}
