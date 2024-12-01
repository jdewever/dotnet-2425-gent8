using Rise.Shared.Cart;

namespace Rise.Shared.Transaction;

public class TransactionDto
{
    public class History
    {
        public required int Id { get; set; }
        public required DateTime Date { get; set; }
        public required string Type { get; set; }
        public required string UserId { get; set; }
        public required IEnumerable<TransactionItemDto> Products { get; set; }
    }
}