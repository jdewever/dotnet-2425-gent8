using Bunit;
using Xunit;
using Rise.Client.Pages;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Rise.Client;
using Rise.Shared.Products;
using Blazored.Toast.Services;
using Rise.Client.Products.AddProduct;
using System.Collections.Generic;

public class AddProductPageTests : TestContext
{
    [Trait("Category", "AddProduct")]
    [Fact]
    public void AddProduct_ShouldRenderCorrectly()
    {
        var testPage = RenderComponent<AddProduct>();

        Assert.Contains("Toevoegen nieuw product", testPage.Markup);
        Assert.NotNull(testPage.Find("form")); 
        Assert.NotNull(testPage.Find("input[placeholder='naam']")); 
    }

    [Fact]
    public void AddProduct_ShouldCallService_WhenFormIsSubmitted()
    {
        var productServiceMock = new Mock<IProductService>();
        var toastServiceMock = new Mock<IToastService>();

        Services.AddSingleton(productServiceMock.Object);
        Services.AddSingleton(toastServiceMock.Object);

        var testProduct = new ProductCreationDTO
        {
            Name = "Test Product",
            ClassRoomCode = "A101",
            Barcode = "1234567890123",
            Description = "Test Description",
            QuantityInStock = 10,
            QuantityOnOrder = 5,
            LowStock = 2,
            IsReservable = true,
            IsHidden = false,
            CategoryIds = new List<int> { 1 }
        };

        var testPage = RenderComponent<AddProduct>();

        testPage.Find("input[placeholder='naam']").Change(testProduct.Name);
        testPage.Find("input[placeholder='lokaal']").Change(testProduct.ClassRoomCode);
        testPage.Find("textarea[placeholder='omschrijving ...']").Change(testProduct.Description);
        testPage.Find("input[placeholder='aantal']").Change(testProduct.QuantityInStock.ToString());

        testPage.Find("form").Submit();

        productServiceMock.Verify(service => service.AddProduct(It.IsAny<ProductCreationDTO>()), Times.Once);
    }

    [Fact]
    public void AddProduct_ShouldShowValidationErrors_WhenFieldsAreEmpty()
    {
        var testPage = RenderComponent<AddProduct>();

        testPage.Find("form").Submit();

        Assert.NotEmpty(testPage.FindAll("span.text-red-500"));
    }
}
