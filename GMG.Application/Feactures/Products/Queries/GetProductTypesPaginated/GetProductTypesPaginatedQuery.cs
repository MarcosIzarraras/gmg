using GMG.Application.Common.Pagination;
using GMG.Domain.Common.Result;
using GMG.Domain.Products.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Products.Queries.GetProductTypesPaginated
{
    public record GetProductTypesPaginatedQuery(Pagination Pagination) : IRequest<PaginationResult<ProductType>>;
}
