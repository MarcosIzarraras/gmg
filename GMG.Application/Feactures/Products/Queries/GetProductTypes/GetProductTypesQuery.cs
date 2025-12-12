using GMG.Domain.Products.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Products.Queries.GetProductTypes
{
    public record GetProductTypesQuery() : IRequest<List<ProductType>>;
}
