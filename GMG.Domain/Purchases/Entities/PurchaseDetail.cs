using GMG.Domain.Common;
using GMG.Domain.Common.Result;
using GMG.Domain.Products.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Domain.Purchases.Entities
{
    public class PurchaseDetail
    {
        public Guid Id { get; set; }
        public Guid PurchaseId { get; set; }
        public Guid ProductId { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Total { get; set; }

        public Product? Product { get; set; }

        private PurchaseDetail() { }
        private PurchaseDetail(Guid purchaseId, Guid productId, double quantity, double unitPrice)
        {
            Id = Guid.NewGuid();
            PurchaseId = purchaseId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Total = quantity * unitPrice;
        }

        public static Result<PurchaseDetail> Create(Guid purchaseId, Guid productId, double quantity, double unitPrice)
        {
            if (quantity <= 0)
                return Result<PurchaseDetail>.Failure("Quantity must be greater than zero.");
            if (unitPrice < 0)
                return Result<PurchaseDetail>.Failure("Unit price cannot be negative.");
            if (purchaseId == Guid.Empty)
                return Result<PurchaseDetail>.Failure("Purchase ID cannot be empty.");
            if (productId == Guid.Empty)
                return Result<PurchaseDetail>.Failure("Product ID cannot be empty.");

            return Result<PurchaseDetail>.Success(new PurchaseDetail(purchaseId, productId, quantity, unitPrice));
        }
    }
}
