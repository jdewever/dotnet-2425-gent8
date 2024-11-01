using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Pages;

public partial class InventoryView : ComponentBase
{
    private IEnumerable<ProductDTO>? lowStockProducts;
    private int lowStockProductCount => lowStockProducts?.Count() ?? 0;
    [Inject] public required IProductService ProductService { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        lowStockProducts = await ProductService.GetProductsHavingLowStock();
    }
}
