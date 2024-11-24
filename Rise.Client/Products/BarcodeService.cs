using System.Net.Http.Json;
using Rise.Shared.Barcodes;

namespace Rise.Client.Products;

public class BarcodeService : IBarcodeService
{
    private readonly HttpClient httpClient;
    private const string barcodeEndpoint = "barcodes";

    public BarcodeService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<BarcodeResponse> GetNewBarcode()
    {
        var response = await httpClient.PostAsJsonAsync(barcodeEndpoint, new { });
        response.EnsureSuccessStatusCode();
        var barcode = await response.Content.ReadFromJsonAsync<BarcodeResponse>();
        return barcode!;
    }

    public async Task<string> GetImage(string barcode)
    {
        var image = await httpClient.GetStringAsync($"barcodes/{barcode}/image");
        return image;
    }

    public bool IsValidBarcode(string barcode)
    {
        throw new NotImplementedException();
    }
}