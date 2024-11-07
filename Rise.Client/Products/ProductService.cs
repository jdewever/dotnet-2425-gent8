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

    public async Task<ProductResponse> GetAllProducts(ProductRequest.Index request)
    {
        var url = "product?";
        var categories = request.CategoryIds;
        request.CategoryIds = null;
        url += request.AsQueryString();

        if (categories != null)
            url = categories.Select(i => i).Aggregate(url, (current, value) => current + $"&CategoryIds={value}");

        var response = await httpClient.GetFromJsonAsync<ProductResponse>(url);

        return response!;
    }

    public async Task<IEnumerable<string>> GetAllLocations()
    {
        var locations = await httpClient.GetFromJsonAsync<IEnumerable<string>>($"product/location");
        return locations!;
    }

    public async Task<ProductDTO> GetProductByBarcode(string? barcode = null)
    {
        try
        {
            var product = await httpClient.GetFromJsonAsync<ProductDTO>($"product/barcode?barcode={barcode}");
            return product!;
        }
        catch (Exception e)
        {
            // Log error
        }
        return null;
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsHavingLowStock()
    {
        var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductDTO>>($"product/lowstock");
        return products!;
    }

    public async Task HideProduct(string barcode)
    {
        await httpClient.DeleteAsync($"product/{barcode}");
    }
}
