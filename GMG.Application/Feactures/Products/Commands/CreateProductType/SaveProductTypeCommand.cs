using GMG.Domain.Common.Result;
using GMG.Domain.Products.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Products.Commands.CreateProductType
{
    public record SaveProductTypeCommand(Guid? Id, string Name) : IRequest<Result<ProductType>>;
}
