namespace Rise.Domain.DomainClasses
{
    public class TransactionItem : Entity
    {
        public required int StockTransactionID { get; init; }
        public required int ProductID { get; init; }
        public required int Quantity { get; init; }
    }
}