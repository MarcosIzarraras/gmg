namespace GMG.Application.Feactures.Sales.Commands.CreateSale
{
    public record CreateSaleDetailDto
    {
        public Guid ProductId { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
