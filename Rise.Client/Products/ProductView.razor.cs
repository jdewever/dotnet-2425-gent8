using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class ProductView : ComponentBase
{
    [Parameter] public required ProductDTO Product { get; set; }
    [Parameter] public string? ImageUrl { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public Boolean Reserve { get; set; } = false;

    [Inject] private ScanBarcodeService ScanBarcodeService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;


    private void NavigateToScan()
    {
        ScanBarcodeService.Barcode = Product.Barcode;
        Navigation.NavigateTo($"/scan");
    }

    private void OnReserverenClick()
    {
        ScanBarcodeService.Barcode = Product.Barcode;
        Navigation.NavigateTo("/reservations");
    }
}