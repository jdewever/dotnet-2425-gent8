using Rise.Shared.Products;
using System.Net.Http.Json;
using Rise.Client.Extensions;

namespace Rise.Client.Products;

public class ProductService : IProductService
{
    private readonly HttpClient httpClient;

    public ProductService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProducts(ProductRequest.Index request)
    {
        var url = "product?";
        var categories = request.CategoryIds;
        request.CategoryIds = null;
        url += $"{request.AsQueryString()}";
        if (categories != null)
            url = categories.Select(i => i).Aggregate(url, (current, value) => current + $"&CategoryIds={value}");
        var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductDTO>>($"{url}{request.AsQueryString()}");
        return products!;
    }

    public async Task<IEnumerable<ProductDTO>> GetSearchedProducts(string? searchTerm = null)
    {
        var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductDTO>>($"product/search?searchTerm={searchTerm}");
        return products!;
    }
}
