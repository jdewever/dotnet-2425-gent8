namespace Rise.Shared.Products;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetAllCategories();
    Task AddCategory(CategoryDTO category);
    Task UpdateCategory(CategoryDTO category);
    Task DeleteCategory(int id);
}