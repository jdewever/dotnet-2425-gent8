namespace Rise.Shared.Products;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetAllCategories();
}