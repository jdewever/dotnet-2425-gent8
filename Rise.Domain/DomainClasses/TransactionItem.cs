namespace Rise.Domain.DomainClasses
{
    public class TransactionItem : Entity
    {
        private int transactionId;
        private int productId;
        private int quantity;

        // EF Core needs a parameterless constructor????
        private TransactionItem() { }

        public TransactionItem(int transactionId, int productId, int quantity)
        {
            this.transactionId = Guard.Against.NegativeOrZero(transactionId, nameof(transactionId));
            this.productId = Guard.Against.NegativeOrZero(productId, nameof(productId));
            this.quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity));
        }

        public int TransactionID => transactionId;
        public int ProductID => productId;
        public int Quantity => quantity;
    }
}