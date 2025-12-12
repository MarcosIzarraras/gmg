namespace GMG.Application.Feactures.Purchases.Commands.CreatePurchase
{
    public record CreatePurchaseDetailDto
    {
        public Guid ProductId { get; init; }
        public double Quantity { get; init; }
        public double UnitPrice { get; init; }
    }
}
