namespace Rise.Shared.User;

public class TransactionDto
{
    public required int Id { get; set; }
    public required DateTime Date { get; set; }
    public required string Type { get; set; }
    public required string UserId { get; set; }
    public required Dictionary<string, int> Products { get; set; }
}