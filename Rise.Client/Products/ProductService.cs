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

    public async Task<ProductDTO> GetProductByBarcode(string barcode)
    {
        var product = await httpClient.GetFromJsonAsync<ProductDTO>($"product/{barcode}");
        return product!;
    }

    public async Task<DashboardDTO> GetDashboardInfo()
    {
        var dash = await httpClient.GetFromJsonAsync<DashboardDTO>($"product/dashboard");
        return dash!;
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
        catch (Exception)
        {
            // Log error
        }
    }

    public async Task UpdateProduct(string barcode, ProductCreationDTO product)
    {
        await httpClient.PutAsJsonAsync($"product/{barcode}", product);
    }

    public async Task<List<ProductDTO>> GetHiddenProducts(ProductRequest.Hidden request)
    {
        return await httpClient.GetFromJsonAsync<List<ProductDTO>>($"product/hidden?" + request.AsQueryString()) ?? [];
    }
}
