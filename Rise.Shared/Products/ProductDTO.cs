using System.ComponentModel.DataAnnotations;

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

    public bool IsHidden { get; set; }

    public required List<CategoryDTO>? Categories { get; set; }
}

// used so a list of category ids so that no complete category object has to be provided
public class ProductCreationDTO
{
    [Required(ErrorMessage = "Naam is verplicht.")]
    [StringLength(50, ErrorMessage = "Naam mag niet langer zijn dan 50 tekens.")]
    public required string Name { get; set; }

    [StringLength(500, ErrorMessage = "Omschrijving mag niet langer zijn dan 500 tekens.")]
    public required string Description { get; set; }
    public required string Barcode { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Aantal in stock moet positief zijn.")]
    public required int QuantityInStock { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Aantal in bestelling moet positief zijn.")]
    public required int QuantityOnOrder { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Min aantal moet positief zijn.")]
    public required int LowStock { get; set; }

    [Required(ErrorMessage = "Lokaal is verplicht.")]
    [RegularExpression(@"^[A-Z]\d{3}$", ErrorMessage = "Lokaal moet beginnen met een letter en gevolgd worden door drie cijfers (bijv. A101).")]
    public required string ClassRoomCode { get; set; }

    public required bool IsReservable { get; set; }
    public required bool IsHidden { get; set; } = false;

    public required List<int> CategoryIds { get; set; }
}