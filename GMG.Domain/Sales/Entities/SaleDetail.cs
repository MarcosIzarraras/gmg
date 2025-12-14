using GMG.Domain.Common;
using GMG.Domain.Common.Result;
using GMG.Domain.Products.Entities;

namespace GMG.Domain.Sales.Entities;

public class SaleDetail : BaseEntity
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public double Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double Total { get; set; }

    public Product? Product { get; set; }

    private SaleDetail() { }

    private SaleDetail(Guid saleId, Guid productId, double quantity, double unitPrice)
    {
        Id = Guid.NewGuid();
        SaleId = saleId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Total = quantity * unitPrice;
    }

    public static Result<SaleDetail> Create(Guid saleId, Guid productId, double quantity, double unitPrice)
    {
        if (quantity <= 0)
            return Result<SaleDetail>.Failure("Quantity must be greater than zero.");

        if (unitPrice < 0)
            return Result<SaleDetail>.Failure("Unit price cannot be negative.");

        if (saleId == Guid.Empty)
            return Result<SaleDetail>.Failure("Sale ID cannot be empty.");

        if (productId == Guid.Empty)
            return Result<SaleDetail>.Failure("Product ID cannot be empty.");

        return Result<SaleDetail>.Success(new SaleDetail(saleId, productId, quantity, unitPrice));
    }
}
