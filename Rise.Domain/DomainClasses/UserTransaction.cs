namespace Rise.Domain.DomainClasses;

public class UserTransaction : Entity
{
    private string userId;
    private string type = null!;

    private static readonly HashSet<string> ValidTransactionTypes = new()
    {
        "ScanIn",
        "ScanOut",
        "AddStock",
    };

    public UserTransaction() { }

    public UserTransaction(string userId, string type)
    {
        SetUserId(userId);
        SetType(type);
    }

    private void SetUserId(string userId)
    {
        this.userId = Guard.Against.NullOrWhiteSpace(userId, nameof(userId));
    }

    private void SetType(string type)
    {
        var validatedType = Guard.Against.NullOrWhiteSpace(type, nameof(type));
        if (!ValidTransactionTypes.Contains(validatedType))
        {
            throw new ArgumentException(
                $"Invalid transaction type. Valid types are: {string.Join(", ", ValidTransactionTypes)}");
        }
        this.type = validatedType;
    }

    public string UserId => userId;
    public string Type => type;
}
