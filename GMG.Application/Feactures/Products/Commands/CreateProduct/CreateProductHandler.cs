using GMG.Application.Common.Interfaces;
using GMG.Application.Common.Persistence;
using GMG.Application.Common.Persistence.Repositories;
using GMG.Domain.Common.Result;
using GMG.Domain.Products.Entities;
using MediatR;

namespace GMG.Application.Feactures.Products.Commands.CreateProduct
{
    public class CreateProductHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService, IUserContext userContext) : IRequestHandler<CreateProductCommand, Result<Product>>
    {
        public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var exist = await unitOfWork.Products.ExistAsync(request.Name);
            if (exist)
                return Result<Product>.Failure("Product with the same name already exists.");

            var productResult = Product.Create(request.Name, request.Description ?? "", request.Price, request.Stock, request.ProductTypeId);

            if (productResult.IsFailure)
                return productResult;

            var imagePaths = new List<string>();
            if (request.Images is not null && request.Images.Any())
            {
                imagePaths = await fileStorageService.SaveFilesAsync(
                    request.Images, 
                    "uploads/products", 
                    userContext.OwnerId.ToString());
            }

            imagePaths.ForEach(imagePath =>
            {
                productResult.Value.AddImage(imagePath);
            });

            await unitOfWork.Products.AddAsync(productResult.Value);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Product>.Success(productResult.Value);
        }
    }
}
