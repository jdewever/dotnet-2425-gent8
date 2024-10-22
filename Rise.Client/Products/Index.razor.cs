using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class Index
{
    private IEnumerable<ProductDto>? products;
    private string? searchTerm;

    [Inject] public required IProductService ProductService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        products = await ProductService.GetAllProducts();
    }
    private async Task OnSearchInput(ChangeEventArgs e)
    {
        searchTerm = e.Value?.ToString();
        products = await ProductService.GetSearchedProducts(searchTerm);
    }
}

