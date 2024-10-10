namespace Rise.Shared.Products;

public class ProductDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public required string Barcode { get; set; }

    public required int QuantityInStock { get; set; }

    public required int QuantityOnOrder { get; set; }

    public required string ClassRoomCode { get; set; }
}
