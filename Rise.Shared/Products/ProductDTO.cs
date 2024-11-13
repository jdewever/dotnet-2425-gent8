namespace Rise.Shared.Products;

public class ProductDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public required string Barcode { get; set; }

    public required int QuantityInStock { get; set; }
    public required int QuantityOnOrder { get; set; }
    public required int LowStock { get; set; }

    public required string ClassRoomCode { get; set; }

    public required bool IsReservable { get; set; }
    public required bool IsHidden { get; set; }
    
    public required List<CategoryDTO>? Categories { get; set; }
}

// used so a list of category ids so that no complete category object has to be provided
public class ProductCreationDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Barcode { get; set; }
    public required int QuantityInStock { get; set; }
    public required int QuantityOnOrder { get; set; }
    public required int LowStock { get; set; }

    public required string ClassRoomCode { get; set; }

    public required bool IsReservable { get; set; }
    public required bool IsHidden { get; set; } = false;

    public required List<int> CategoryIds { get; set; }
}