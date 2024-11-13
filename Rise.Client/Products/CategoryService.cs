using System.Net.Http.Json;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;

    public CategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDTO>>("category");
        return result!;
    }

    public async Task AddCategory(CategoryDTO category)
    {
        // todo: add validation
        await _httpClient.PostAsJsonAsync("category", category);
    }

    public async Task DeleteCategory(int id)
    {
        await _httpClient.DeleteAsync($"category/{id}");
    }
}