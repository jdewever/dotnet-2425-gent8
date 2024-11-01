namespace Rise.Domain.DomainClasses
{
    public class Category : Entity
    {
        private string name = null!;
        private List<Product> products = [];

        private Category() { }

        public Category(string name)
        {
            this.name = Guard.Against.NullOrWhiteSpace(name);
        }

        public string Name
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
