using GMG.Application.Common.Persistence.Repositories;
using GMG.Domain.Common.Result;
using GMG.Domain.Sales.Entities;
using MediatR;

namespace GMG.Application.Feactures.Sales.Commands.CreateSale
{
    public class CreateSaleHandler(IRepository<Sale> saleRepository) : IRequestHandler<CreateSaleCommand, Result<Sale>>
    {
        public async Task<Result<Sale>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var total = request.Details.Sum(d => d.UnitPrice * d.Quantity);

            var saleResult = Sale.Create(total, request.CustomerId);

            if (saleResult.IsFailure)
                return saleResult;

            var sale = saleResult.Value;

            var detailsResult = request.Details
                .Select(d => sale.AddDetail(d.ProductId, d.Quantity, d.UnitPrice))
                .ToList();

            var detailsError = detailsResult
                .Where(r => r.IsFailure)
                .Select(r => r.Error)
                .ToList();

            if (detailsError is not null && detailsError.Any())
                return Result<Sale>.Failure(string.Join(". ", detailsError));

            await saleRepository.AddAsync(sale);

            return Result<Sale>.Success(sale);
        }
    }
}
