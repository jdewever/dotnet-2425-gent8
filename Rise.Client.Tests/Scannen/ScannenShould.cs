using Rise.Shared.Products;
using Rise.Client.Scan;
using Xunit.Abstractions;
using Shouldly;
using System.Linq;
using System;
using Bunit;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Web;


namespace Rise.Client.Products;

public class ScannenShould : TestContext
{

    private readonly ITestOutputHelper _output;
    public ScannenShould(ITestOutputHelper outputHelper)
    {
        _output = outputHelper;
        Services.AddXunitLogger(outputHelper);
		    Services.AddScoped<IProductService, FakeProductService>();
	    	Services.AddScoped<ICategoryService, FakeCategoryService>();
        Services.AddSingleton(new BarcodeService());
    }

    [Fact]
    public void ShowsBarcode()
    {
        // Arrange
        Func<string, int> getProductCountByBarcode = barcode => 0;

        var cut = RenderComponent<ScanProduct>(parameters => parameters
            .Add(p => p.GetProductCountByBarcode, getProductCountByBarcode)
        );

        // Act
        var input = cut.Find("input");

        // Assert
        input.GetAttribute("placeholder").ShouldBe("123456789");
    }

    [Fact]
    public void ShowProductOnBarcodeValid()
    {
        // Arrange
        Func<string, int> getProductCountByBarcode = barcode => 0;

        var barcodeService = Services.GetRequiredService<BarcodeService>();

        var cut = RenderComponent<ScanProduct>(parameters => parameters
            .Add(p => p.GetProductCountByBarcode, getProductCountByBarcode)
        );

        // Act
        cut.InvokeAsync(() =>
        {
            barcodeService.Barcode = "Barcode 1";
            cut.Render();
        }).Wait();
 
        // Assert
        var img = cut.Find("img");
        img.ShouldNotBeNull();
        img.GetAttribute("src").ShouldBe("images/testafbeelding.png");

        var label = cut.Find("label.quantity-in-stock");
        label.TextContent.ShouldBe("1");
    }

    [Fact]
    public void ShowProductOnBarcodeInvalid()
    {
        // Arrange
        Func<string, int> getProductCountByBarcode = barcode => 0;

        var barcodeService = Services.GetRequiredService<BarcodeService>();

        var cut = RenderComponent<ScanProduct>(parameters => parameters
            .Add(p => p.GetProductCountByBarcode, getProductCountByBarcode)
        );

        // Act
        cut.InvokeAsync(() =>
        {
            barcodeService.Barcode = "Barcode 10";
            cut.Render();
        }).Wait();

        // Assert
        var img = cut.FindAll("img");
        img.ShouldBeEmpty();

        var span = cut.Find("span");
        span.TextContent.ShouldBe("Product ingeven ...");

        var label = cut.Find("label.quantity-in-stock");
        label.TextContent.ShouldBe("");
    }
}