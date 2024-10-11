using Rise.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Domain.DomainClasses
{
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
    }

}
