namespace Rise.Shared.Transaction;

public class TransactionDTO
{
    public required int Id { get; set; }
    public required DateTime Date { get; set; }
    public required string Type { get; set; }
    public required string UserId { get; set; }
    public required IEnumerable<TransactionItemDTO> Products { get; set; }
}