using GMG.Application.Feactures.Interfaces;
using GMG.Domain.Common.Result;
using GMG.Domain.Products.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Products.Commands.UpdateProduct
{
    public class UpdateProductHandler(IProductRepository productRepository) : IRequestHandler<UpdateProductCommand, Result<Product>>
    {
        public async Task<Result<Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(request.Id);
            if (product is null)
            {
                return Result<Product>.Failure("Product not found.");
            }

            var productResult = product.Update(request.Name, request.Description, request.Price, request.Stock, request.ProductTypeId);

            if (productResult.IsFailure)
                return productResult;

            await productRepository.UpdateAsync(product);

            return Result<Product>.Success(product);
        }
    }
}
