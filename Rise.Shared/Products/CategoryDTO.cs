namespace Rise.Shared.Products;

public class CategoryDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public List<ProductDto>? Products { get; set; }
}