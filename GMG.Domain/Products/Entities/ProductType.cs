using GMG.Domain.Common;
using GMG.Domain.Common.Result;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;

namespace GMG.Domain.Products.Entities
{
    public class ProductType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        private ProductType() { }
        private ProductType(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public static Result<ProductType> Create(string name) 
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<ProductType>.Failure("Product Type Name is required.");

            return Result<ProductType>.Success(new ProductType(name));
        }

        public Result<ProductType> Update(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<ProductType>.Failure("Product Type Name is required.");

            Name = name;

            return Result<ProductType>.Success(this);
        }
    }
}
