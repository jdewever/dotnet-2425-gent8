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
        private bool isReservable = default!;
        private bool isHidden = default!;
        private string imageUrl = string.Empty;
        private List<Category> categories = default!;

        public Product() { }

        public Product(string name, string description, int quantityInStock, int quantityOnOrder, int lowStock, string classRoomCode, string barcode, bool isReservable, bool isHidden, string imageUrl)
        {
            Name = name;
            this.description = description;
            this.quantityInStock = quantityInStock;
            this.quantityOnOrder = quantityOnOrder;
            this.lowStock = lowStock;
            this.classRoomCode = classRoomCode;
            this.barcode = barcode;
            this.isReservable = isReservable;
            this.isHidden = isHidden;
            this.imageUrl = imageUrl;
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

        public bool IsReservable
        {
            get => isReservable;
            set => isReservable = value;
        }

        public bool IsHidden
        {
            get => isHidden;
            set => isHidden = value;
        }

        public List<Category> Categories
        {
            get => categories;
            set => categories = Guard.Against.Null(value, nameof(Categories));
        }

        public string ImageUrl
        {
            get => imageUrl;
            set => imageUrl = Guard.Against.NullOrWhiteSpace(value);
        }
    }

}