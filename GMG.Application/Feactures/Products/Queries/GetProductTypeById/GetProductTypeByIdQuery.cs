using GMG.Domain.Products.Entities;
using MediatR;

namespace GMG.Application.Feactures.Products.Queries.GetProductTypeById
{
    public record GetProductTypeByIdQuery(Guid Id) : IRequest<ProductType?>;
}
