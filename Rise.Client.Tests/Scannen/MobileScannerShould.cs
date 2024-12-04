using Rise.Shared.Products;
using Rise.Client.Scan;
using Xunit;
using Shouldly;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using System.Linq;
using Rise.Client.Cart;
using Rise.Shared.Cart;
using Rise.Client.Scannen;

namespace Rise.Client.Products;

public class MobileScannerShould : TestContext
{
    private ScanService? barcodeService;
    private IRenderedComponent<ScanProduct>? scanProductComponent;

    public MobileScannerShould()
    {
        Services.AddScoped<IProductService, FakeProductService>();
        Services.AddScoped<ICategoryService, FakeCategoryService>();
        Services.AddScoped<ICartService, FakeCartService>();
        Services.AddSingleton(new ScanService());
        Services.AddBlazoredLocalStorage();

        Initialize();
    }

    private void Initialize()
    {
        barcodeService = Services.GetRequiredService<ScanService>();

        RenderMobileScannerComponent();
    }

    private void RenderMobileScannerComponent()
    {
        scanProductComponent = RenderComponent<ScanProduct>(parameters => parameters
            .Add(p => p.GetProductCountByBarcode, barcode => 0) // Dummy implementation for testing purposes
            .Add(p => p.AddProduct, EventCallback.Factory.Create<(ProductDTO, int)>(this, async productInfo => await Task.CompletedTask))
        );
    }

    [Fact(Skip = "Temporarily skipping this test")]
    public async Task StartScannerAutomaticallyInMobileView()
    {
        // Arrange
        scanProductComponent!.Instance.IsMobileView = true;
        scanProductComponent!.Instance.ShowScanner = true;

        // Act
        await scanProductComponent.InvokeAsync(() => scanProductComponent.Render());

        // Assert
        var scannerComponent = scanProductComponent.FindComponent<BlazorBarcodeScanner.ZXing.JS.BarcodeReader>();
        scannerComponent.ShouldNotBeNull();
        scannerComponent.Instance.StartCameraAutomatically.ShouldBeTrue();
    }

    [Fact(Skip = "Temporarily skipping this test")]
    public async Task ShowBarcodeInputWhenNotInMobileView()
    {
        // Arrange
        scanProductComponent!.Instance.IsMobileView = false;
        scanProductComponent!.Instance.ShowScanner = false;

        // Act
        await scanProductComponent.InvokeAsync(() => scanProductComponent.Render());

        // Assert
        var inputField = scanProductComponent.Find("input[type='text']");
        inputField.ShouldNotBeNull();
    }

    [Fact(Skip = "Temporarily skipping this test")]
    public async Task ReceiveBarcodeCorrectly()
    {
        // Arrange
        scanProductComponent!.Instance.IsMobileView = true;
        scanProductComponent!.Instance.ShowScanner = true;

        // Act
        await scanProductComponent.InvokeAsync(() =>
        {
            barcodeService!.Barcode = "123456789";
            scanProductComponent!.Render();
        });

        // Assert
        var label = scanProductComponent.Find("span");
        label.TextContent.ShouldBe("Product ingeven ...");
    }

    [Fact(Skip = "Temporarily skipping this test")]
    public async Task ToggleTorchOptionInMobileScanner()
    {
        // Arrange
        scanProductComponent!.Instance.IsMobileView = true;
        scanProductComponent!.Instance.ShowScanner = true;

        // Act
        await scanProductComponent.InvokeAsync(() => scanProductComponent.Render());

        // Assert
        var scannerComponent = scanProductComponent.FindComponent<BlazorBarcodeScanner.ZXing.JS.BarcodeReader>();
        scannerComponent.ShouldNotBeNull();
        scannerComponent.Instance.ShowToggleTorch.ShouldBeTrue();
    }
}
