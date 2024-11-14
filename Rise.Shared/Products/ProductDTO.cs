using System.ComponentModel.DataAnnotations;

namespace Rise.Shared.Products;

public class ProductDTO
{
    public required int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "Barcode is required.")]
    public required string Barcode { get; set; }

    [Required(ErrorMessage = "In stock is required.")]
    public required int QuantityInStock { get; set; }

    [Required(ErrorMessage = "On order is required.")]
    public required int QuantityOnOrder { get; set; }

    [Required(ErrorMessage = "Low stock is required.")]
    public required int LowStock { get; set; }

    [Required(ErrorMessage = "Classroomcode is required.")]
    public required string ClassRoomCode { get; set; }

    [Required(ErrorMessage = "Reservable is required.")]
    public required bool IsReservable { get; set; }
    public required bool IsHidden { get; set; }

    [Required(ErrorMessage = "Category is required.")]
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