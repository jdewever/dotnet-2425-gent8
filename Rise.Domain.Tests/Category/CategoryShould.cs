using Rise.Domain.DomainClasses;
using Shouldly;

namespace Rise.Domain.Tests.Categorys;
public class CategoryShould
{
    [Fact]
    public void BeCreatedWithValidName()
    {
        var category = new Category("Electronics");

        category.Name.ShouldBe("Electronics");
    }

    [Fact]
    public void HaveEmptyProductListByDefault()
    {
        var category = new Category("Electronics");

        category.Products.ShouldNotBeNull();
        category.Products.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NotAllowInvalidCategoryName(string invalidName)
    {
        Action act = () => new Category(invalidName);

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void NotAllowNullProductList()
    {
        var category = new Category("Electronics");

        Action act = () => category.Products = null;

        act.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void AllowUpdatingProductList()
    {
        var products = new List<string> { "Laptop", "Phone" };
        var category = new Category("Electronics");

        category.Products = products;

        category.Products.ShouldBe(products);
        category.Products.ShouldContain("Laptop");
        category.Products.ShouldContain("Phone");
    }
}
