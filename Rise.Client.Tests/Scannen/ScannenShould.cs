using Rise.Shared.Products;
using Rise.Shared.Cart;
using Rise.Client.Scan;
using Rise.Client.Cart;
using Shouldly;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Rise.Client.Products;
using Blazored.Toast;


namespace Rise.Client.Scannen;

public class ScannenShould : TestContext
{

    private ScanService? barcodeService;
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
        Services.AddBlazoredToast();
        Services.AddSingleton(new ScanService());
        Services.AddBlazoredLocalStorage();

        Initialize();
    }

    private void Initialize()
    {
        cartItems = new List<CartItem>();
        barcodeService = Services.GetRequiredService<ScanService>();
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
        img.GetAttribute("src").ShouldContain("/api/proxy/image?url=");

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
        
        var increaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("+1"));
        increaseButton.ShouldNotBeNull();
        increaseButton.Click();

        
        var addButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
        addButton.ShouldNotBeNull();
        addButton.Click();

        scanTableComponent!.Render();

        // Assert
        var productName = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("Product 1")); 
        productName.ShouldNotBeNull();
        var quantity = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("1"));
        quantity.ShouldNotBeNull();
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

        var increaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("+1"));
        increaseButton.ShouldNotBeNull();
        increaseButton.Click();
        increaseButton.Click();

        var addButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
        addButton.ShouldNotBeNull();
        addButton.Click();

        scanTableComponent!.Render();

        // Assert
        var productName = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("Product 2"));
        productName.ShouldNotBeNull();
        var quantity = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("2"));
        quantity.ShouldNotBeNull();
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

        var increaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("+1"));
        increaseButton.ShouldNotBeNull();
        increaseButton.Click();
        increaseButton.Click();

        var decreaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent == "-1");
        decreaseButton.ShouldNotBeNull();
        decreaseButton.Click();

        var addButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
        addButton.ShouldNotBeNull();
        addButton.Click();

        scanTableComponent!.Render();

        // Assert
        var productName = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("Product 3"));
        productName.ShouldNotBeNull();
        var quantity = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("1"));
        quantity.ShouldNotBeNull();
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

        scanTableComponent!.Render();
        
        // Assert
        var productName = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("Product 1"));
        productName.ShouldBeNull();
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

        scanTableComponent!.Render();

        // Assert
        var productName1 = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("Product 1"));
        productName1.ShouldNotBeNull();
        var quantity1 = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("1"));
        quantity1.ShouldNotBeNull();

        var productName2 = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("Product 2"));
        productName2.ShouldNotBeNull();
        var quantity2 = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("1"));
        quantity2.ShouldNotBeNull();
    }

    [Fact]
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

        scanTableComponent!.Render();

        var productName = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("Product 2"));
        productName.ShouldNotBeNull();
        var quantity = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("1"));
        quantity.ShouldNotBeNull();

        var removeButton = scanTableComponent!.FindAll("button").FirstOrDefault(b => b.OuterHtml.Contains("text-red-600"));
        removeButton.ShouldNotBeNull();
        removeButton.Click();

        scanTableComponent!.Render();

        // Assert
        var productName2 = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("Product 2"));
        productName2.ShouldBeNull();
    }

    [Fact]
    public async Task AddProductMoreThenStock(){
        // Arrange
        cartItems!.Clear();

        // Act
        await scanProductComponent!.InvokeAsync(() =>
        {
            barcodeService!.Barcode = "Barcode 1";
            scanProductComponent!.Render();
        });

        var increaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent == "+1");
        increaseButton.ShouldNotBeNull();
        increaseButton.Click();
        increaseButton.Click();
        increaseButton.Click();

        var addButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
        addButton.ShouldNotBeNull();
        addButton.Click();

        scanTableComponent!.Render();

        // Assert
        var productName = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("Product 1"));
        productName.ShouldNotBeNull();
        var quantity = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("3"));
        quantity.ShouldNotBeNull();
        quantity.GetAttribute("class")!.ShouldContain("text-red-700");

        var errorMessages = scanTableComponent!.FindAll("label").FirstOrDefault(td => td.TextContent.Contains("Niet alle producten in stock"));
    }

    [Fact]
    public async Task AddProductMoreThenStockAndChangeToCorrectValue(){
        // Arrange
        cartItems!.Clear();

        // Act
        await scanProductComponent!.InvokeAsync(() =>
        {
            barcodeService!.Barcode = "Barcode 1";
            scanProductComponent!.Render();
        });

        var increaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent == "+1");
        increaseButton.ShouldNotBeNull();
        increaseButton.Click();
        increaseButton.Click();

        var decreaseButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent == "-1");
        decreaseButton.ShouldNotBeNull();
        decreaseButton.Click();

        var addButton = scanProductComponent!.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Voeg toe"));
        addButton.ShouldNotBeNull();
        addButton.Click();

        scanTableComponent!.Render();

        // Assert
        var productName = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("Product 1"));
        productName.ShouldNotBeNull();
        var quantity = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains('1'));
        quantity.ShouldNotBeNull();
        quantity.GetAttribute("class")!.ShouldNotContain("text-red-700");

        var errorMessages = scanTableComponent!.FindAll("label").FirstOrDefault(td => td.TextContent.Contains("Niet alle producten in stock"));
        errorMessages.ShouldBeNull();

        var productTd = scanTableComponent!.FindAll("td").FirstOrDefault(td => td.TextContent.Contains("Product 1"));
        productTd.ShouldNotBeNull();
        productTd.Click();

        await scanProductComponent!.InvokeAsync(() =>
        {
            scanProductComponent!.Render();
        });

        var input = scanProductComponent!.Find("input");
        input.GetAttribute("value").ShouldBe("Barcode 1");
    }
}