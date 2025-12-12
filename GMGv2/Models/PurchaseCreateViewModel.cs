using GMG.Application.Feactures.Purchases.Commands.CreatePurchase;

namespace GMGv2.Models
{
    public class PurchaseCreateViewModel
    {
        public List<CreatePurchaseDetailDto> Details { get; set; } = new();

        public double Total => Details.Sum(d => d.Quantity * d.UnitPrice);
    }
}
