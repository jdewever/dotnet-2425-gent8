using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public class FakeCategoryService: ICategoryService
{
    public Task AddCategory(CategoryDTO category)
    {
        throw new System.NotImplementedException();
    }

    public Task UpdateCategory(CategoryDTO category)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteCategory(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<CategoryDTO>> GetAllCategories()
    {
        var categories = Enumerable.Range(1, 5)
            .Select(i => new CategoryDTO { Id = i, Name = $"Category {i}"});
        return Task.FromResult(categories);
    }
}