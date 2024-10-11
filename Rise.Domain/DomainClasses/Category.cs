using Rise.Domain.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Domain.Products
{
    public class Category : Entity
    {
        private string name = default!;
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public Category() { }

        public Category(string name)
        {
            this.name = name;
        }

        public required string Name
        {
            get => name;
            set => name = Guard.Against.NullOrWhiteSpace(value);
        }

    }
}
