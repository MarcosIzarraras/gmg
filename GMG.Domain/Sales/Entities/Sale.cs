using GMG.Domain.Common;
using GMG.Domain.Common.Result;
using GMG.Domain.Customers.Entities;

namespace GMG.Domain.Sales.Entities;

public class Sale : BaseEntity
{
    public double Total { get; set; }
    public bool IsCancelled { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    // Optional relationship with Customer
    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    private List<SaleDetail> _details = new();
    public IReadOnlyList<SaleDetail> Details
        => _details.AsReadOnly();

    private Sale() { }

    private Sale(double total, Guid? customerId = null)
    {
        Id = Guid.NewGuid();
        Total = total;
        CustomerId = customerId;
        IsCancelled = false;
        CancelledAt = null;
    }

    public static Result<Sale> Create(double total, Guid? customerId = null)
    {
        if (total <= 0)
            return Result<Sale>.Failure("Total must be greater than zero.");

        return Result<Sale>.Success(new Sale(total, customerId));
    }

    public Result<SaleDetail> AddDetail(Guid productId, double quantity, double unitPrice)
    {
        if (IsCancelled)
            return Result<SaleDetail>.Failure("Cannot add details to a cancelled sale.");

        var result = SaleDetail.Create(this.Id, productId, quantity, unitPrice);

        if (result.IsSuccess)
            _details.Add(result.Value);

        return result;
    }

    public Result<Sale> Cancel()
    {
        if (IsCancelled)
            return Result<Sale>.Failure("Sale is already cancelled.");

        IsCancelled = true;
        CancelledAt = DateTime.UtcNow;

        return Result<Sale>.Success(this);
    }
}
