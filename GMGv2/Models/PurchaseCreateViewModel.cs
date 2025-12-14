using GMG.Application.Feactures.Purchases.Commands.CreatePurchase;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GMGv2.Models
{
    public class PurchaseCreateViewModel
    {
        public Guid SupplierId { get; set; }
        public DateTime PurchaseAt { get; set; } = DateTime.Now;
        public DateTime? DeliveryAt { get; set; }

        public List<CreatePurchaseDetailDto> Details { get; set; } = new();

        public double Total => Details.Sum(d => d.Quantity * d.UnitPrice);

        public List<SelectListItem> Suppliers { get; set; } = new();
    }
}
