namespace Rise.Domain.DomainClasses
{
    public class Cart : Entity
    {
        private string userId = default!;
        private Dictionary<Product, int> checkoutItems = new Dictionary<Product, int>();
        // TODO sommige constructors mogen weg
        public Cart() { }

        public Cart(string userId)
        {
            this.userId = userId;
        }

        public Cart(string userId, Dictionary<Product, int> checkoutItems)
        {
            this.userId = userId;
            this.checkoutItems = checkoutItems;
        }

        public required string UserId
        {
            get => userId;
            set => userId = Guard.Against.NullOrWhiteSpace(value);
        }

        public Dictionary<Product, int> CheckoutItems
        {
            get => checkoutItems;
            set => checkoutItems = Guard.Against.Null(value);
        }
    }

}