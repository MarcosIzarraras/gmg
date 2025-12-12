using GMG.Domain.Common;
using GMG.Domain.Common.Result;

namespace GMG.Domain.Purchases.Entities
{
    public class Purchase : BaseEntity
    {
        public double Total { get; set; }

        private List<PurchaseDetail> _details = new();
        public IReadOnlyList<PurchaseDetail> Details 
            => _details.AsReadOnly();

        private Purchase() { }

        private Purchase(double total) { 
            Id = Guid.NewGuid();
            Total = total; 
        }

        public static Result<Purchase> Create(double total) 
        {
            if (total <= 0)
                return Result<Purchase>.Failure("Total must be greater than zero.");

            return Result<Purchase>.Success(new Purchase(total));
        }

        public Result<PurchaseDetail> AddDetail(Guid productId, double quantity, double unitPrice)
        {
            var result = PurchaseDetail.Create(this.Id, productId, quantity, unitPrice);
            
            if (result.IsSuccess)
                _details.Add(result.Value);
            
            return result;
        }
    }
}
