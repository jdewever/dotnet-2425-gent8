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
}