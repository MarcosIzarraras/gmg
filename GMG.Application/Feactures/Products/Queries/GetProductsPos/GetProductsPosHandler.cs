using GMG.Application.Common.Persistence.Repositories;
using MediatR;

namespace GMG.Application.Feactures.Products.Queries.GetProducts
{
    public class GetProductsPosHandler(IProductRepository productRepository) : IRequestHandler<GetProductsPosQuery, List<ProductPosDto>>
    {
        public async Task<List<ProductPosDto>> Handle(GetProductsPosQuery request, CancellationToken cancellationToken)
        {
            var products = await productRepository.ListAsync(x => x.ProductType);
            return products.Select(p => new ProductPosDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ProductTypeId = p.ProductType.Id,
                ProductTypeName = p.ProductType.Name
            }).ToList();
        }
    }
}
