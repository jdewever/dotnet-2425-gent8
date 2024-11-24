namespace Rise.Domain.DomainClasses
{
    public class TransactionItem
    {
        public UserTransaction Transaction { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }

        public TransactionItem() { }

        public TransactionItem(UserTransaction transaction, Product product, int quantity)
        {
            Transaction = Guard.Against.Null(transaction, nameof(Transaction));
            Product = Guard.Against.Null(product, nameof(Product));
            Quantity = Guard.Against.NegativeOrZero(quantity, nameof(Quantity));
        }
    }
}
