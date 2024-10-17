using System.Collections;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class Index
{
    private IEnumerable<ProductDto>? products;
    private IEnumerable<CategoryDTO>? categories;
    private IList<CategoryDTO>? _selectedCategoriesList;

    [Inject] public required IProductService ProductService { get; set; }
    [Inject] public required ICategoryService CategoryService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        categories = await CategoryService.GetAllCategories();
        products = await ProductService.GetAllProducts();
    }

    private async Task<IEnumerable<CategoryDTO>> SearchCategory(string searchTerm)
    {
        return await Task.FromResult(categories!.Where(category => category.Name.Contains(searchTerm)));
    }
}

