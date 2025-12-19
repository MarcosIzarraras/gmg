using GMG.Application.Common.Dtos;
using GMG.Domain.Common.Result;
using GMG.Domain.Products.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Products.Commands.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, double Price, double Stock, Guid ProductTypeId, List<FileUploadDto> Images) : IRequest<Result<Product>>;
}
