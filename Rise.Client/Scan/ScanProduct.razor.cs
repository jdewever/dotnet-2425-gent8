using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;
using BlazorBarcodeScanner.ZXing.JS;
using Microsoft.JSInterop;

namespace Rise.Client.Scan;

public partial class ScanProduct : ComponentBase
{
    [Parameter] public ProductDTO? Product { get; set; }
    [Parameter] public EventCallback<(ProductDTO, int)> AddProduct { get; set; }
    [Parameter] public required Func<string, int> GetProductCountByBarcode { get; set; }
    [Inject] private ScanService ScanService { get; set; } = null!;
    [Inject] private IProductService ProductService { get; set; } = null!;
    [Parameter] public bool IsMobileView { get; set; }
    [Parameter] public bool ShowScanner { get; set; }
    [Parameter] public EventCallback<bool> ShowScannerChanged { get; set; }
    [Parameter] public bool ShowQuantityModal { get; set; }
    [Parameter] public EventCallback<bool> ShowQuantityModalChanged { get; set; }

    private int Quantity = 0;

    protected override async Task OnInitializedAsync()
    {
        // Subscribe to the OnBarcodeChanged event
        // When the barcode changes, searchProductByBarcode is called and the product is updated and quantity is reset
        ScanService.OnBarcodeChanged += async (sender, args) => await SearchProductByBarcode();
        if (!string.IsNullOrEmpty(ScanService.Barcode))
        {
            await SearchProductByBarcode();
        }
    }

    private async Task SearchProductByBarcode()
    {
        if (!string.IsNullOrEmpty(ScanService.Barcode))
        {
            Product = await ProductService.GetProductByBarcode(ScanService.Barcode);
            Quantity = 0;
            await ShowQuantityModalChanged.InvokeAsync(true);
            await ShowScannerChanged.InvokeAsync(false);
            StateHasChanged();
        }
    }
    private async void OnAddProduct()
    {
        if (Product == null)
        {
            return;
        }
        await AddProduct.InvokeAsync((Product, Quantity));
        Quantity = 0;
        ScanService.Barcode = "";
        Product = null;
        await ShowQuantityModalChanged.InvokeAsync(false);
        await ShowScannerChanged.InvokeAsync(true);
        StateHasChanged();
    }


    private int GetProductCount()
    {
        return GetProductCountByBarcode.Invoke(Product?.Barcode ?? "");
    }

    private async void LocalReceivedBarcodeText(BarcodeReceivedEventArgs args)
    {
        BarcodeService.Barcode = args.BarcodeText;
        await ShowScannerChanged.InvokeAsync(false);
        await ShowQuantityModalChanged.InvokeAsync(true);
        StateHasChanged();
    }
}