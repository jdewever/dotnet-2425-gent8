using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Scan;

public partial class ScanProduct : ComponentBase
{
    [Parameter] public ProductDTO? Product { get; set; }
    [Parameter] public EventCallback<(ProductDTO, int)> AddProduct { get; set; }
    [Parameter] public required Func<string, int> GetProductCountByBarcode { get; set; }
    [Inject] private BarcodeService BarcodeService { get; set; } = null!;
    [Inject] private IProductService ProductService { get; set; } = null!;
    private int Quantity = 0;

    protected override async Task OnInitializedAsync()
    {
        // Subscribe to the OnBarcodeChanged event
        // When the barcode changes, searchProductByBarcode is called and the product is updated and quantity is reset
        BarcodeService.OnBarcodeChanged += async (sender, args) => await SearchProductByBarcode();
        if (!string.IsNullOrEmpty(BarcodeService.Barcode))
        {
            await SearchProductByBarcode();
        }
    }

    private async Task SearchProductByBarcode()
    {
        if (!string.IsNullOrEmpty(BarcodeService.Barcode))
        {
            Product = await ProductService.GetProductByBarcode(BarcodeService.Barcode);
            Quantity = 0;
            StateHasChanged();
        }
    }
    private void OnAddProduct()
    {
        if (Product == null)
        {
            return;
        }
        AddProduct.InvokeAsync((Product, Quantity));
        Quantity = 0;
    }


    private int GetProductCount()
    {
        return GetProductCountByBarcode.Invoke(Product?.Barcode ?? "");
    }
}