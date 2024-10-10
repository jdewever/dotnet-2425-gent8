namespace Rise.Domain.Products;
public class Product : Entity
{
    private string name = default!;
    private string description = default!;
    private int quantityInStock = default!;
    private int quantityOnOrder = default!;
    private string classRoomCode = default!;
    private string barcode = default!;

    public Product() { } 

    public Product(string name, string description, int quantityInStock, int quantityOnOrder, string classRoomCode, string barcode)
    {
        Name = name;
        Description = description;
        QuantityInStock = quantityInStock;
        QuantityOnOrder = quantityOnOrder;
        ClassRoomCode = classRoomCode;
        Barcode = barcode;
    }

    public required string Name
    {
        get => name;
        set => name = Guard.Against.NullOrWhiteSpace(value);
    }

    public string Description
    {
        get => description;
        set => description = Guard.Against.NullOrWhiteSpace(value);
    }

    public int QuantityInStock
    {
        get => quantityInStock;
        set => quantityInStock = Guard.Against.Negative(value, nameof(QuantityOnOrder));
    }

    public int QuantityOnOrder
    {
        get => quantityOnOrder;
        set => quantityOnOrder = Guard.Against.Negative(value, nameof(QuantityOnOrder));
    }

    public string ClassRoomCode
    {
        get => classRoomCode;
        set => classRoomCode = Guard.Against.NullOrWhiteSpace(value);
    }

    public string Barcode
    {
        get => barcode;
        set => barcode = Guard.Against.NullOrWhiteSpace(value);
    }
}

