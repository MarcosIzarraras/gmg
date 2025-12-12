using GMG.Domain.Common;
using GMG.Domain.Common.Result;

namespace GMG.Domain.Products.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public double Stock { get; set; }
        public Guid ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = null!;

        private Product() { }
        private Product(string name, string description, double price, double stock, Guid productTypeId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            ProductTypeId = productTypeId;
            Stock = stock;
        }

        public static Result<Product> Create(string name, string description, double price, double stock, Guid productTypeId)
        {
            if (string.IsNullOrEmpty(name))
                return Result<Product>.Failure("Product name cannot be null or empty.");
            if (price < 0)
                return Result<Product>.Failure("Product price cannot be negative.");
            if (productTypeId == Guid.Empty)
                return Result<Product>.Failure("Product type ID cannot be empty.");
           
            return Result<Product>.Success(new Product(name, description, price, stock, productTypeId));
        }

        public Result<Product> Update(string name, string description, double price, double stock, Guid productTypeId)
        {
            if (string.IsNullOrEmpty(name))
                return Result<Product>.Failure("Product name cannot be null or empty.");
            if (price < 0)
                return Result<Product>.Failure("Product price cannot be negative.");
            if (productTypeId == Guid.Empty)
                return Result<Product>.Failure("Product type ID cannot be empty.");

            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            ProductTypeId = productTypeId;

            return Result<Product>.Success(this);
        }
    }
}
