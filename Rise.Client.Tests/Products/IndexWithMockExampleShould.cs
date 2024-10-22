using Rise.Shared.Products;
using Xunit.Abstractions;
using Shouldly;
using NSubstitute;
using System.Threading.Tasks;
using System.Linq;

namespace Rise.Client.Products;

/// <summary>
/// Same as <see cref="IndexShould"/> using mocking instead of faking.
/// https://nsubstitute.github.io
/// </summary>
public class IndexWithMockExampleShould : TestContext
{
    public IndexWithMockExampleShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
    }

    [Fact]
    public void ShowsProducts()
    {
        var products = Enumerable.Range(1, 5)
            .Select(i => new ProductDto { Id = i, Name = $"Product {i}", Barcode = $"Barcode {i}", Description = $"Description {i}", ClassRoomCode = $"ClassRoom {i}", QuantityInStock = i, QuantityOnOrder = i, Categories = null });
        var categories = Enumerable.Range(1, 5)
            .Select(i => new CategoryDTO { Id = i, Name = $"Category {i}" });
        
        var productServiceMock = Substitute.For<IProductService>();
        productServiceMock.GetAllProducts(Arg.Any<ProductRequest.Index>())
            .Returns(Task.FromResult(products));
        
        var categoryServiceMock = Substitute.For<ICategoryService>();
        categoryServiceMock.GetAllCategories()
            .Returns(Task.FromResult(categories));

        Services.AddScoped<IProductService>(_ => productServiceMock);
        Services.AddScoped<ICategoryService>(_ => categoryServiceMock);

        var cut = RenderComponent<Index>();

        cut.FindAll("div.product").Count.ShouldBe(5);
    }
}
