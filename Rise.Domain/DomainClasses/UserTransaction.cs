namespace Rise.Domain.DomainClasses;

public class UserTransaction : Entity
{
    private string userId;
    private string type = null!;
    private DateTime transactionDate;
    private List<TransactionItem> transactionItems;
    private List<Product> products;
    

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

    private void setDate(DateTime date)
    {
        this.transactionDate = Guard.Against.Null(date, nameof(date));
    }

    private void setTransactionItems(List<TransactionItem> transactionItems)
    {
        this.transactionItems = Guard.Against.Null(transactionItems, nameof(transactionItems));
    }

    private void setProducts(List<Product> products)
    {
        this.products = Guard.Against.Null(products, nameof(products));
    }

    public string UserId => userId;
    public string Type => type;
    public DateTime TransactionDate => transactionDate;
    public List<TransactionItem> TransactionItems => transactionItems;
    public List<Product> Products => products;
}
