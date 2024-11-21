using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Components.ProductTable;

public partial class ProductDetailModal : ComponentBase
{
    [Parameter] public required ProductDTO SelectedProduct { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Inject] private BarcodeService BarcodeService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Parameter] public Boolean Reserve { get; set; } = false;

    private void NavigateToScan()
    {
        BarcodeService.Barcode = SelectedProduct.Barcode;
        Navigation.NavigateTo($"/scan");
    }

    private void OnReserverenClick()
    {
        OnClick.InvokeAsync();
    }

    private void ProductManagement()
    {
        Navigation.NavigateTo("/products/management");
    }
}