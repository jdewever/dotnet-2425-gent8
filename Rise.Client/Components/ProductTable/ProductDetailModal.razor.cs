using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Components.ProductTable;

public partial class ProductDetailModal : ComponentBase
{
    [Parameter] public required ProductDTO SelectedProduct { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Inject] private BarcodeService BarcodeService { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }


    private void NavigateToScan()
    {
        BarcodeService.Barcode = SelectedProduct.Barcode;
        Navigation.NavigateTo($"/scan");
    }
}