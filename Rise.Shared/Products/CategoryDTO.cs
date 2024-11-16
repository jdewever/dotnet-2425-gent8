namespace Rise.Shared.Products;

public class CategoryDTO
{
    public int Id { get; set; } // no required attribute because this is an auto-incremented value (otherwise no category can be added)
    public required string Name { get; set; }
    public List<ProductDTO>? Products { get; set; }
}