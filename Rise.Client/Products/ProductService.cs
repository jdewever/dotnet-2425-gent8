using Rise.Shared.Products;
using System.Net.Http.Json;
using Rise.Client.Extensions;
using System.IO.Pipelines;
using System.Net.Http.Headers;
using System.Net;

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

    public async Task AddProduct(ProductCreationDTO product)
    {
        await httpClient.PostAsJsonAsync("product", product);
    }

    public async Task ToggleHideProduct(string barcode)
    {
        await httpClient.PostAsync($"product/{barcode}/hide", null);
    }

    public async Task DeleteProduct(string barcode)
    {
        try
        {
            var result = await httpClient.DeleteAsync($"product/{barcode}");
        }
        catch (Exception e)
        {
            // Log error
        }
    }

}
