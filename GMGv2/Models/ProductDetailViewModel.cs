using Microsoft.AspNetCore.Mvc.Rendering;

namespace GMGv2.Models
{
    public class ProductDetailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public double Stock { get; set; }
        public Guid ProductTypeId { get; set; }

        public List<SelectListItem> ProductTypes { get; set; } = new List<SelectListItem>();
        public List<ProductImageViewModel>? Images { get; set; }
        public List<IFormFile>? NewImages { get; set; }
        public List<string>? ImageIdsToDelete { get; set; }
    }

    public class ProductImageViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
    }
}
