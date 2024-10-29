using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Scan;

public partial class Index : ComponentBase
{
    private List<ProductDTO> products = new List<ProductDTO>();
    private ProductDTO? selectedProduct = null;

    [Inject] private ILocalStorageService localStorage { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        var storedProducts = await localStorage.GetItemAsync<List<ProductDTO>>("products");
        if (storedProducts != null)
        {
            products = storedProducts;
        }
    }

    private async Task addProduct((ProductDTO product, int quantity) productInfo)
    {
        for (int i = 0; i < productInfo.quantity; i++)
        {
            products.Add(productInfo.product);
        }
        await localStorage.SetItemAsync("products", products);
    }

    private int getProductCountByBarcode(string barcode)
    {
        return barcode != "" ? products.Count(p => p.Barcode == barcode) : 0;
    }

}