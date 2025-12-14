using GMG.Application.Feactures.Products.Queries.GetProducts;
using GMG.Application.Feactures.Sales.Commands.CreateSale;
using GMG.Domain.Products.Entities;

namespace GMGv2.Models
{
    public class SaleCreateViewModel
    {
        public Guid? CustomerId { get; set; }
        
        public List<ProductType?>? ProductTypes { get; set; } = new();
        public List<ProductPosDto?>? Products { get; set; } = new();
        public List<CreateSaleDetailDto?>? Details { get; set; } = new();
    }
}
