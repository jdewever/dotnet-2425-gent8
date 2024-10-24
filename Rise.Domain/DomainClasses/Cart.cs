namespace Rise.Domain.DomainClasses
{
    public class Cart : Entity
    {
        private string userId = default!;
        private List<CheckoutItem> checkoutItems = default!;

        // TODO sommige constructors mogen weg
        public Cart() { }

        public Cart(string userId)
        {
            this.userId = userId;
        }

        public Cart(string userId, List<CheckoutItem> checkoutItems)
        {
            this.userId = userId;
            this.checkoutItems = checkoutItems;
        }

        public required string UserId
        {
            get => userId;
            set => userId = Guard.Against.NullOrWhiteSpace(value);
        }

        public List<CheckoutItem> CheckoutItems
        {
            get => checkoutItems;
            set => checkoutItems = Guard.Against.Null(value);
        }
    }

}