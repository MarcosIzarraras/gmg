using GMG.Domain.Common.Result;
using GMG.Domain.Purchases.Entities;
using MediatR;

namespace GMG.Application.Feactures.Purchases.Commands.CreatePurchase
{
    public record CreatePurchaseCommand(List<CreatePurchaseDetailDto> details) : IRequest<Result<Purchase>>;
}
