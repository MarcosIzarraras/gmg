using GMG.Application.Common.Persistence;
using GMG.Application.Common.Persistence.Repositories;
using GMG.Domain.Common.Result;
using GMG.Domain.Products.Entities;
using MediatR;

namespace GMG.Application.Feactures.Products.Commands.CreateProduct
{
    public class CreateProductHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCommand, Result<Product>>
    {
        public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var exist = await unitOfWork.Products.ExistAsync(request.Name);
            if (exist)
                return Result<Product>.Failure("Product with the same name already exists.");

            var productResult = Product.Create(request.Name, request.Description ?? "", request.Price, request.Stock, request.ProductTypeId);

            if (productResult.IsFailure)
                return productResult;

            await unitOfWork.Products.AddAsync(productResult.Value);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Product>.Success(productResult.Value);
        }
    }
}
