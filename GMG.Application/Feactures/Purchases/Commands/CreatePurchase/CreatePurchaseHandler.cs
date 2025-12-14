using GMG.Application.Feactures.Interfaces;
using GMG.Domain.Common.Result;
using GMG.Domain.Purchases.Entities;
using MediatR;

namespace GMG.Application.Feactures.Purchases.Commands.CreatePurchase
{
    public class CreatePurchaseHandler(IRepository<Purchase> purchaseRepository) : IRequestHandler<CreatePurchaseCommand, Result<Purchase>>
    {
        public async Task<Result<Purchase>> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
        {
            var purchaseResult = Purchase.Create(request.details.Sum(d => d.UnitPrice * d.Quantity), request.Supplier, request.PurchaseAt, request.DeliveryAt);

            if (purchaseResult.IsFailure)
                return purchaseResult;

            var purchase = purchaseResult.Value;

            var detailsResult = request.details
                .Select(d => purchase.AddDetail(d.ProductId, d.Quantity, d.UnitPrice))
                .ToList();

            var detailsError = detailsResult
                .Where(r => r.IsFailure)
                .Select(r => r.Error)
                .ToList();

            if (detailsError is not null && detailsError.Any())
                return Result<Purchase>.Failure(string.Join(". ", detailsError));
            
            await purchaseRepository.AddAsync(purchase);

            return Result<Purchase>.Success(purchase);
        }
    }
}
