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
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;


namespace Rise.Client.Products;

public class ScannenShould : TestContext
{

    private readonly ITestOutputHelper _output;
    private readonly BarcodeService barcodeService;
    private List<ProductDTO> addedProducts;
    private Func<string, int> getProductCountByBarcode;

    public ScannenShould(ITestOutputHelper outputHelper)
    {
        _output = outputHelper;
        Services.AddXunitLogger(outputHelper);
		    Services.AddScoped<IProductService, FakeProductService>();
	    	Services.AddScoped<ICategoryService, FakeCategoryService>();
        Services.AddSingleton(new BarcodeService());
        barcodeService = Services.GetRequiredService<BarcodeService>();
        getProductCountByBarcode = barcode => addedProducts.Count(p => p.Barcode == barcode);
    }

    [Fact]
    public void ShowsBarcode()
    {
        // Arrange
        addedProducts = new List<ProductDTO>();

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
        addedProducts = new List<ProductDTO>();

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
        addedProducts = new List<ProductDTO>();
        
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

    [Fact]
    public void UpdateStockOnProductAddOnce()
    {
        // Arrange
        addedProducts = new List<ProductDTO>();

        var cut = RenderComponent<ScanProduct>(parameters => parameters
            .Add(p => p.GetProductCountByBarcode, getProductCountByBarcode)
            .Add(p => p.AddProduct, EventCallback.Factory.Create<(ProductDTO, int)>(this, product => addedProducts.Add(product.Item1)))
        );

        // Act
        cut.InvokeAsync(() =>
        {
            barcodeService.Barcode = "Barcode 1";
            cut.Render();
        }).Wait();

        
        var increaseButton = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("+"));
        increaseButton.Click();

        
        var addButton = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
        addButton.Click();

        // Assert
        var label = cut.Find("label.quantity-in-stock");
        label.TextContent.ShouldBe("0");

        addedProducts.Count.ShouldBe(1);
        addedProducts.First().Barcode.ShouldBe("Barcode 1");
    }

    [Fact]
public void UpdateStockOnProductAddTwice()
{
    // Arrange
    addedProducts = new List<ProductDTO>();

    var cut = RenderComponent<ScanProduct>(parameters => parameters
        .Add(p => p.GetProductCountByBarcode, getProductCountByBarcode)
        .Add(p => p.AddProduct, EventCallback.Factory.Create<(ProductDTO, int)>(this, product => addedProducts.Add(product.Item1)))
    );

    // Act
    cut.InvokeAsync(() =>
    {
        barcodeService.Barcode = "Barcode 2";
        cut.Render();
    }).Wait();

    var increaseButton = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("+"));
    increaseButton.Click();
    increaseButton.Click();

    var addButton = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
    addButton.Click();

    var label = cut.Find("label.quantity-in-stock");
    label.TextContent.ShouldBe("0");
    
}

}