namespace Rise.Domain.DomainClasses
{
    public class TransactionItem : Entity
    {
        public int TransactionID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        public TransactionItem() { }

        public TransactionItem(int transactionId, int productId, int quantity)
        {
            this.TransactionID = Guard.Against.NegativeOrZero(transactionId, nameof(transactionId));
            this.ProductID = Guard.Against.NegativeOrZero(productId, nameof(productId));
            this.Quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity));
        }
    }
}
