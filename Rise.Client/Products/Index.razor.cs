using System.Collections;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class Index
{
    private IEnumerable<ProductDTO>? products;
    private IEnumerable<CategoryDTO>? categories;
    private IList<int>? _selectedCategories;
    private string? searchTerm;

    [Inject] public required IProductService ProductService { get; set; }
    [Inject] public required ICategoryService CategoryService { get; set; }
    [Parameter, SupplyParameterFromQuery(Name = "SelectedCategory")] public int[] SelectedCategoryList { get; set; } = [];

    private async Task OnSearchInput(ChangeEventArgs e)
    {
        searchTerm = e.Value?.ToString();
        products = await ProductService.GetSearchedProducts(searchTerm);
    }

    protected override async Task OnParametersSetAsync()
    {
        _selectedCategories = SelectedCategoryList.ToList();
        ProductRequest.Index request = new()
        {
            CategoryIds = SelectedCategoryList,
        };
        categories = await CategoryService.GetAllCategories();
        products = await ProductService.GetAllProducts(request);
    }
}
