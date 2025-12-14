namespace GMG.Application.Feactures.Sales.Queries.GetSalesPaginated
{
    public class SalesPaginatedDto
    {
        public Guid Id { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string Total { get; set; } = string.Empty;
        public string Client { get; set; } = string.Empty;
    }
}
