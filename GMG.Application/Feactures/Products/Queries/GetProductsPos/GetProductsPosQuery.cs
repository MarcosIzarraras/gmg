using MediatR;

namespace GMG.Application.Feactures.Products.Queries.GetProducts
{
    public class GetProductsPosQuery() : IRequest<List<ProductPosDto>>;
}
