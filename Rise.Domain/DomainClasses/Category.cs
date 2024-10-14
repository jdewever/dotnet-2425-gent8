namespace Rise.Domain.DomainClasses
{
    public class Category : Entity
    {
        private string name = default!;
        private List<Product> products = default!;

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

        public List<Product> Products
        {
            get => products;
            set => products = Guard.Against.Null(value, nameof(Products));
        }

    }
}
