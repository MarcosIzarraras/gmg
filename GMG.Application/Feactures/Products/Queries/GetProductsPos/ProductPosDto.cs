namespace GMG.Application.Feactures.Products.Queries.GetProducts
{
    public class ProductPosDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public Guid ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
    }
}
