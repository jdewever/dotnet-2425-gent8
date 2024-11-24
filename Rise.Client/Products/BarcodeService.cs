using System.Net.Http.Json;
using Rise.Shared.Barcodes;

namespace Rise.Client.Products;

public class BarcodeService : IBarcodeService
{
    private readonly HttpClient httpClient;

    public BarcodeService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<BarcodeResponse> GetNewBarcode()
    {
        var barcode = await httpClient.GetFromJsonAsync<BarcodeResponse>("barcodes");
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