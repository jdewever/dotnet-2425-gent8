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
    private List<int> _categoryIds = new();

    [Required(ErrorMessage = "Naam is verplicht.")]
    [StringLength(50, ErrorMessage = "Naam mag niet langer zijn dan 50 tekens.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Omschrijving is verplicht.")]
    [StringLength(500, ErrorMessage = "Omschrijving mag niet langer zijn dan 500 tekens.")]
    public string Description { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Aantal in stock moet positief zijn.")]
    public int QuantityInStock { get; set; } = 0;

    [Range(0, int.MaxValue, ErrorMessage = "Aantal in bestelling moet positief zijn.")]
    public int QuantityOnOrder { get; set; } = 0;

    [Range(0, int.MaxValue, ErrorMessage = "Min aantal moet positief zijn.")]
    public int LowStock { get; set; } = 0;

    [Required(ErrorMessage = "Lokaal is verplicht.")]
    public string ClassRoomCode { get; set; } = string.Empty;

    public bool IsReservable { get; set; } = false;
    public bool IsHidden { get; set; } = false;

    [Required]
    public List<int> CategoryIds
    {
        get => _categoryIds;
        set
        {
            if (value.Count < 0 || value.Count > 3)
            {
                throw new ValidationException("Aantal categorieën moet tussen 0 en 3 zijn.");
            }
            _categoryIds = value;
        }
    }
}