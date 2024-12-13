using Rise.Shared.Products;
using Xunit.Abstractions;
using Shouldly;
using System.Linq;

namespace Rise.Client.Products;

/// <summary>
/// These tests are written entirely in C#.
/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
public class IndexShould : TestContext
{
    public IndexShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        Services.AddScoped<IProductService, FakeProductService>();
        Services.AddScoped<ICategoryService, FakeCategoryService>();
    }

    [Fact(Skip = "Temporarily skipping this test")]
    public void ShowsProducts()
    {
        var cut = RenderComponent<Index>();
        cut.FindAll("div").Where(div => div.ClassList.Contains("product")).Count().ShouldBe(5);
    }
}
