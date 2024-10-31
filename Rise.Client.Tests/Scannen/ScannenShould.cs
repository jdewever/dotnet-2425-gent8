using Rise.Shared.Products;
using Rise.Shared.Cart;
using Rise.Client.Scan;
using Rise.Client.Cart;
using Xunit.Abstractions;
using Shouldly;
using System.Linq;
using System;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;


namespace Rise.Client.Products;

public class ScannenShould : TestContext
{

    private BarcodeService? barcodeService;
    private List<CartItem>? cartItems;
    private Func<string, int>? getProductCountByBarcode;
    private EventCallback<(ProductDTO, int)> addProduct;
    private EventCallback<CartItem> removeProduct;
    
    private IRenderedComponent<ScanProduct>? scanProductComponent;
    private IRenderedComponent<ScanTable>? scanTableComponent;

    public ScannenShould()
    {
        Services.AddScoped<IProductService, FakeProductService>();
        Services.AddScoped<ICategoryService, FakeCategoryService>();
        Services.AddScoped<ICartService, FakeCartService>();
        Services.AddSingleton(new BarcodeService());

        Initialize();
    }

    private void Initialize()
    {
        cartItems = new List<CartItem>();
        barcodeService = Services.GetRequiredService<BarcodeService>();
        getProductCountByBarcode = barcode => cartItems.Where(p => p.Product.Barcode == barcode).Sum(p => p.Quantity);
        
        addProduct = EventCallback.Factory.Create<(ProductDTO, int)>(this, async productInfo =>
        {
            cartItems.Add(new CartItem { Product = productInfo.Item1, Quantity = productInfo.Item2 });
            await Task.CompletedTask;
        });
        
        removeProduct = EventCallback.Factory.Create<CartItem>(this, async item =>
        {
            var cartItem = cartItems.Find(p => p.Product.Barcode == item.Product.Barcode);
            if (cartItem != null)
            {
                cartItems.Remove(cartItem);
            }
            await Task.CompletedTask;
        });

        RenderIndexComponent();
    }

    private void RenderIndexComponent()
    {
        scanProductComponent = RenderComponent<ScanProduct>(parameters => parameters
            .Add(p => p.GetProductCountByBarcode, getProductCountByBarcode)
            .Add(p => p.AddProduct, addProduct)
        );
        scanTableComponent = RenderComponent<ScanTable>(parameters => parameters
            .Add(p => p.CartItems, cartItems)
            .Add(p => p.RemoveProduct, removeProduct)
        );
    }
    

    [Fact]
    public void ShowsBarcode()
    {
        // Arrange
        cartItems!.Clear();

        // Act
        var input = scanProductComponent!.Find("input");

        // Assert
        input.GetAttribute("placeholder").ShouldBe("123456789");
    }

    [Fact]
    public async Task ShowProductOnBarcodeValid()
    {
        // Arrange
        cartItems!.Clear();

        // Act
        await scanProductComponent!.InvokeAsync(() =>
        {
            barcodeService!.Barcode = "Barcode 1";
            scanProductComponent!.Render();
        });

        // Assert
        var img = scanProductComponent!.Find("img");
        img.ShouldNotBeNull();
        img.GetAttribute("src").ShouldBe("images/testafbeelding.png");

        var label = scanProductComponent!.Find("label.quantity-in-stock");
        label.TextContent.ShouldBe("1");
    }

    [Fact]
    public async Task ShowProductOnBarcodeInvalid()
    {
        // Arrange
        cartItems!.Clear();

        // Act
        await scanProductComponent!.InvokeAsync(() =>
        {
            barcodeService!.Barcode = "xyz";
            scanProductComponent!.Render();
        });

        // Assert
        var img = scanProductComponent!.FindAll("img");
        img.ShouldBeEmpty();

        var span = scanProductComponent!.Find("span");
        span.TextContent.ShouldBe("Product ingeven ...");

        var label = scanProductComponent!.Find("label.quantity-in-stock");
        label.TextContent.ShouldBe("");
    }

    [Fact]
    public async Task UpdateStockOnProductAddOnce()
    {
        // Arrange
        cartItems!.Clear();

        // Act
        await scanProductComponent!.InvokeAsync(() =>
        {
            barcodeService!.Barcode = "Barcode 1";
            scanProductComponent!.Render();
        });
        
        var increaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("+"));
        increaseButton.ShouldNotBeNull();
        increaseButton.Click();

        
        var addButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
        addButton.ShouldNotBeNull();
        addButton.Click();

        // Assert
        var label = scanProductComponent!.Find("label.quantity-in-stock");
        label.TextContent.ShouldBe("0");

        cartItems.Count.ShouldBe(1);
        cartItems.First().Product.Barcode.ShouldBe("Barcode 1");
    }

    [Fact]
    public async Task UpdateStockOnProductAddTwice()
    {
        // Arrange
        cartItems!.Clear();

        // Act
        await scanProductComponent!.InvokeAsync(() =>
        {
            barcodeService!.Barcode = "Barcode 2";
            scanProductComponent!.Render();
        });

        var increaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("+"));
        increaseButton.ShouldNotBeNull();
        increaseButton.Click();
        increaseButton.Click();

        var addButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
        addButton.ShouldNotBeNull();
        addButton.Click();

        // Assert
        var label = scanProductComponent!.Find("label.quantity-in-stock");
        label.TextContent.ShouldBe("0");

        cartItems.Count.ShouldBe(1);
        cartItems.All(p => p.Product.Barcode == "Barcode 2").ShouldBeTrue();
    }

    [Fact]
    public async Task UpdateStockOnProductAddTwiceAndRemoveOnce()
    {
        // Arrange
        cartItems!.Clear();

        // Act
        await scanProductComponent!.InvokeAsync(() =>
        {
            barcodeService!.Barcode = "Barcode 3";
            scanProductComponent!.Render();
        });

        var increaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("+"));
        increaseButton.ShouldNotBeNull();
        increaseButton.Click();
        increaseButton.Click();

        var decreaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("-"));
        decreaseButton.ShouldNotBeNull();
        decreaseButton.Click();

        var addButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
        addButton.ShouldNotBeNull();
        addButton.Click();

        

        // Assert
        var label = scanProductComponent!.Find("label.quantity-in-stock");
        label.TextContent.ShouldBe("2");

        cartItems.Count.ShouldBe(1);
        cartItems.All(p => p.Product.Barcode == "Barcode 3").ShouldBeTrue();
    }

    [Fact] 
    public async Task UpdateStockOnProductAddTwiceAndRemoveTwice()
    {
        // Arrange
        cartItems!.Clear();

        // Act
        await scanProductComponent!.InvokeAsync(() =>
        {
            barcodeService!.Barcode = "Barcode 1";
            scanProductComponent!.Render();
        });

        var increaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("+"));
        increaseButton.ShouldNotBeNull();
        increaseButton.Click();
        increaseButton.Click();

        var decreaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("-"));
        decreaseButton.ShouldNotBeNull();
        decreaseButton.Click();
        decreaseButton.Click();

        var addButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
        addButton.ShouldNotBeNull();
        addButton.HasAttribute("disabled").ShouldBeFalse();

        // Assert
        var label = scanProductComponent!.Find("label.quantity-in-stock");
        label.TextContent.ShouldBe("1");

        cartItems.Count.ShouldBe(0);
    }

    [Fact]
    public async Task UpdateStockOnDifferentProductAdd(){
        // Arrange
        cartItems!.Clear();

        // Act
        await scanProductComponent!.InvokeAsync(() =>
        {
            barcodeService!.Barcode = "Barcode 1";
            scanProductComponent!.Render();
        });

        var increaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("+"));
        increaseButton.ShouldNotBeNull();
        increaseButton.Click();

        var addButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
        addButton.ShouldNotBeNull();
        addButton.Click();

        await scanProductComponent!.InvokeAsync(() =>
        {
            barcodeService!.Barcode = "Barcode 2";
            scanProductComponent!.Render();
        });

        var increaseButton2 = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("+"));
        increaseButton2.ShouldNotBeNull();
        increaseButton2.Click();

        var addButton2 = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
        addButton2.ShouldNotBeNull();
        addButton2.Click();

        // Assert
        var label = scanProductComponent!.Find("label.quantity-in-stock");
        label.TextContent.ShouldBe("1");

        cartItems.Count.ShouldBe(2);
    }

    [Fact (Skip = "Not working yet")]
    public async Task AddProductToCartAndRemoveIt(){
        // Arrange
        cartItems!.Clear();

        // Act
        await scanProductComponent!.InvokeAsync(() =>
        {
            barcodeService!.Barcode = "Barcode 2";
            scanProductComponent!.Render();
        });

        var increaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("+"));
        increaseButton.ShouldNotBeNull();
        increaseButton.Click();

        var addButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
        addButton.ShouldNotBeNull();
        addButton.Click();

        var label = scanProductComponent!.Find("label.quantity-in-stock");
        label.TextContent.ShouldBe("0");

        cartItems.Count.ShouldBe(1);

        var removeButton = scanTableComponent!.FindAll("button").FirstOrDefault(b => b.OuterHtml.Contains("text-red-600"));
        removeButton.ShouldNotBeNull();
        removeButton.Click();

        cartItems.Count.ShouldBe(0);

    }
}