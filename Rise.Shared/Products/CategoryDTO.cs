using System.ComponentModel.DataAnnotations;

namespace Rise.Shared.Products;

public class CategoryDTO
{
    public int Id { get; set; } // no required attribute because this is an auto-incremented value (otherwise no category can be added)

    [Required(ErrorMessage = "Naam is verplicht.")]
    [StringLength(50, ErrorMessage = "Naam mag niet langer zijn dan 50 tekens.")]
    public required string Name { get; set; }
    public List<ProductDTO>? Products { get; set; }
}