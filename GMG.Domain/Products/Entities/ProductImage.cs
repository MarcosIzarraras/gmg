using GMG.Domain.Common.Result;

namespace GMG.Domain.Products.Entities
{
    public class ProductImage
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public string Path { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }

        public Product? Product { get; private set; }

        private ProductImage() { }
        private ProductImage(Guid productId, string path)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Path = path;
            CreatedAt = DateTime.UtcNow;
        }

        public static Result<ProductImage> Create(Guid productId, string path)
        {
            return new Validation()
                .Required(productId != Guid.Empty, "Product ID is required.")
                .Required(!string.IsNullOrWhiteSpace(path), "Image path is required.")
                .Build(() => new ProductImage(productId, path));
        }
    }
}
