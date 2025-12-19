using GMG.Application.Feactures.Products.Commands.CreateProduct;
using GMG.Domain.Products.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GMGv2.Models
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public double InitialStock { get; set; }
        public Guid ProductTypeId { get; set; }

        public List<IFormFile>? Images { get; set; }
        public List<SelectListItem> ProductTypes { get; set; } = new();
    }
}
