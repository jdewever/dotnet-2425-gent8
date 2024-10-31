namespace Rise.Domain.DomainClasses
{
    public class Product : Entity
    {
        private string name = default!;
        private string description = default!;
        private int quantityInStock = default!;
        private int quantityOnOrder = default!;
        private int lowStock = default!;
        private string classRoomCode = default!;
        private string barcode = default!;
        private List<Category> categories = default!;
        // public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public Product() { }

        public Product(string name, string description, int quantityInStock, int quantityOnOrder, int lowStock, string classRoomCode, string barcode)
        {
            Name = name;
            this.description = description;
            this.quantityInStock = quantityInStock;
            this.quantityOnOrder = quantityOnOrder;
            this.lowStock = lowStock;
            this.classRoomCode = classRoomCode;
            this.barcode = barcode;
        }

        public string Name
        {
            get => name;
            set => name = Guard.Against.NullOrWhiteSpace(value);
        }

        public string Description
        {
            get => description;
            set => description = Guard.Against.NullOrWhiteSpace(value);
        }

        public int QuantityInStock
        {
            get => quantityInStock;
            set => quantityInStock = Guard.Against.Negative(value, nameof(QuantityOnOrder));
        }

        public int QuantityOnOrder
        {
            get => quantityOnOrder;
            set => quantityOnOrder = Guard.Against.Negative(value, nameof(QuantityOnOrder));
        }

        public int LowStock
        {
            get => lowStock;
            set => lowStock = Guard.Against.Negative(value, nameof(LowStock));
        }

        public string ClassRoomCode
        {
            get => classRoomCode;
            set => classRoomCode = Guard.Against.NullOrWhiteSpace(value);
        }

        public string Barcode
        {
            get => barcode;
            set => barcode = Guard.Against.NullOrWhiteSpace(value);
        }

        public List<Category> Categories
        {
            get => categories;
            set => categories = Guard.Against.Null(value, nameof(Categories));
        }
    }

}