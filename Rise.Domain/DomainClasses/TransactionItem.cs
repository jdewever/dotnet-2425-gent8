namespace Rise.Domain.DomainClasses
{
    public class TransactionItem : Entity
    {
        public required int TransactionID { get; init; }
        public required int ProductID { get; init; }
        public required int Quantity { get; init; }
    }
}