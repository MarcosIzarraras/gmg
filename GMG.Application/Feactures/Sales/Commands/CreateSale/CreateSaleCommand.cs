using GMG.Domain.Common.Result;
using GMG.Domain.Sales.Entities;
using MediatR;

namespace GMG.Application.Feactures.Sales.Commands.CreateSale
{
    public record CreateSaleCommand(List<CreateSaleDetailDto> Details, Guid? CustomerId) : IRequest<Result<Sale>>;
}
