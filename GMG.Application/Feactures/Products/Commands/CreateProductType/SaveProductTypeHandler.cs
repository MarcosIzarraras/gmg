using GMG.Application.Common.Persistence;
using GMG.Application.Common.Persistence.Repositories;
using GMG.Domain.Common.Result;
using GMG.Domain.Products.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Products.Commands.CreateProductType
{
    public class SaveProductTypeHandler : IRequestHandler<SaveProductTypeCommand, Result<ProductType>>
    {
        private readonly IProductTypeRepository productTypeRepository;
        private readonly IUnitOfWork unitOfWork;

        public SaveProductTypeHandler(IProductTypeRepository productTypeRepository, IUnitOfWork unitOfWork)
        {
            this.productTypeRepository = productTypeRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductType>> Handle(SaveProductTypeCommand request, CancellationToken cancellationToken)
        {
            Result<ProductType> result;
            if (request.Id is null)
            {
                var exist = await productTypeRepository.ExistsByNameAsync(request.Name);

                if (exist)
                    return Result<ProductType>.Failure($"Product type with name '{request.Name}' already exists.");

                result = ProductType.Create(request.Name);

                if (result.IsFailure)
                    return result;

                await productTypeRepository.AddAsync(result.Value);
            }
            else
            {
                var productType = await productTypeRepository.GetByIdAsync(request.Id.Value);

                if (productType is null)
                    return Result<ProductType>.Failure("Product type not found.");

                result = productType.Update(request.Name);

                if (result.IsFailure)
                    return result;

            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<ProductType>.Success(result.Value);
        }
    }
}
