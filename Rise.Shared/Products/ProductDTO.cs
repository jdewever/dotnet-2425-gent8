using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

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

    public required string ImageUrl { get; set; }

    public required List<CategoryDTO>? Categories { get; set; }
}

// used so a list of category ids so that no complete category object has to be provided
public class ProductCreationDTO
{
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

    public int CategoryOneId { get; set; } = -1;
    public int CategoryTwoId { get; set; } = -1;
    public int CategoryThreeId { get; set; } = -1;

    public string ImageUrl { get; set; } = string.Empty;

    public ProductCreationDTO() {}

    public ProductCreationDTO(ProductDTO product)
    {
        Name = product.Name;
        Description = product.Description;
        Barcode = product.Barcode;
        QuantityInStock = product.QuantityInStock;
        QuantityOnOrder = product.QuantityOnOrder;
        LowStock = product.LowStock;
        ClassRoomCode = product.ClassRoomCode;
        IsReservable = product.IsReservable;
        IsHidden = product.IsHidden;
        ImageUrl = product.ImageUrl;
        if (product.Categories != null)
        {
            SetCategoryIds(product.Categories);
        }
    }

    public List<int> GetCategoryIds()
    {
        List<int> categoryIds = [];
        if (CategoryOneId != -1)
        {
            categoryIds.Add(CategoryOneId);
        }
        if (CategoryTwoId != -1)
        {
            categoryIds.Add(CategoryTwoId);
        }
        if (CategoryThreeId != -1)
        {
            categoryIds.Add(CategoryThreeId);
        }
        return categoryIds;
    }

    private void SetCategoryIds(List<CategoryDTO> categories)
    {
        CategoryOneId = categories.ElementAtOrDefault(0)?.Id ?? -1;
        CategoryTwoId = categories.ElementAtOrDefault(1)?.Id ?? -1;
        CategoryThreeId = categories.ElementAtOrDefault(2)?.Id ?? -1;
    }
}
