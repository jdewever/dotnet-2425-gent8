namespace Rise.Domain.DomainClasses;

public class UserTransaction : Entity
{
    private string userId = null!;
    private string type = null!;
    private List<TransactionItem> transactionItems = null!;
    private List<Product> products = null!;
    

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
    public UserTransaction(string userId, string type, List<TransactionItem> transactionItems, List<Product> products)
    {
        SetUserId(userId);
        SetType(type);
        SetProducts(products);
        SetTransactionItems(transactionItems);
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

    public void SetTransactionItems(List<TransactionItem> transactionItems)
    {
        this.transactionItems = Guard.Against.Null(transactionItems, nameof(transactionItems));
    }

    private void SetProducts(List<Product> products)
    {
        this.products = Guard.Against.Null(products, nameof(products));
    }

    public string UserId => userId;
    public string Type => type;
    public List<TransactionItem> TransactionItems => transactionItems;
    public List<Product> Products => products;
}
