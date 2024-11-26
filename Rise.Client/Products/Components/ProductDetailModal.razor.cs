using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;
using Rise.Client.Scan;

namespace Rise.Client.Products.Components;

public partial class ProductDetailModal : ComponentBase
{
    [Parameter] public required ProductDTO SelectedProduct { get; set; }
    [Parameter] public EventCallback OnCloseClick { get; set; }
    [Parameter] public EventCallback OnActionClick { get; set; }
    [Inject] private ScanService ScanService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Parameter] public bool Reserve { get; set; } = false;

    private void ProductManagement()
    {
        Navigation.NavigateTo($"/products/management/?barcode={SelectedProduct.Barcode}");
    }
}