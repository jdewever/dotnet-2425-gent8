namespace Rise.Domain.Products;
public class Product : Entity
{
    private string name = default!;

    public required string Name
    {
        get => name;
        set => name = Guard.Against.NullOrWhiteSpace(value);
    }

    private string description = default!;

    public required string Description
    {
        get => description;
        set => description = Guard.Against.NullOrWhiteSpace(value);
    }
}

