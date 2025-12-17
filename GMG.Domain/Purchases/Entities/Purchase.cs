using GMG.Domain.Common;
using GMG.Domain.Common.Result;
using GMG.Domain.Suppliers.Entities;

namespace GMG.Domain.Purchases.Entities
{
    public class Purchase : OwnBranch
    {
        public double Total { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        // Relationship with Supplier
        public Guid SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        private List<PurchaseDetail> _details = new();
        public IReadOnlyList<PurchaseDetail> Details
            => _details.AsReadOnly();

        private Purchase() { }

        private Purchase(double total, Guid supplierId, DateTime purchaseDate, DateTime? deliveryDate = null)
        {
            Id = Guid.NewGuid();
            Total = total;
            SupplierId = supplierId;
            PurchaseDate = purchaseDate;
            DeliveryDate = deliveryDate;
        }

        public static Result<Purchase> Create(double total, Guid supplierId, DateTime purchaseDate, DateTime? deliveryDate = null)
        {
            if (total <= 0)
                return Result<Purchase>.Failure("Total must be greater than zero.");

            if (supplierId == Guid.Empty)
                return Result<Purchase>.Failure("Supplier ID cannot be empty.");

            if (purchaseDate > DateTime.UtcNow)
                return Result<Purchase>.Failure("Purchase date cannot be in the future.");

            if (deliveryDate.HasValue && deliveryDate.Value < purchaseDate)
                return Result<Purchase>.Failure("Delivery date cannot be before purchase date.");

            return Result<Purchase>.Success(new Purchase(total, supplierId, purchaseDate, deliveryDate));
        }

        public Result<PurchaseDetail> AddDetail(Guid productId, double quantity, double unitPrice)
        {
            var result = PurchaseDetail.Create(this.Id, productId, quantity, unitPrice);

            if (result.IsSuccess)
                _details.Add(result.Value);

            return result;
        }

        public Result<Purchase> SetDeliveryDate(DateTime deliveryDate)
        {
            if (deliveryDate < PurchaseDate)
                return Result<Purchase>.Failure("Delivery date cannot be before purchase date.");

            DeliveryDate = deliveryDate;
            return Result<Purchase>.Success(this);
        }
    }
}
