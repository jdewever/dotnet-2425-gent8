using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class Index
{
    private IEnumerable<ProductDto>? products;
    private IEnumerable<CategoryDTO>? categories;

    [Inject] public required IProductService ProductService { get; set; }
    [Inject] public required ICategoryService CategoryService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        categories = await CategoryService.GetAllCategories();
        products = await ProductService.GetAllProducts();
    }
}

