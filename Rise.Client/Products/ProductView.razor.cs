using Microsoft.AspNetCore.Components;
using Rise.Client.Scan;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class ProductView : ComponentBase
{
    [Parameter] public required ProductDTO Product { get; set; }
    [Parameter] public string? ImageUrl { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public bool Reserve { get; set; } = false;

    [Inject] private ScanService ScanService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private bool isReservableProductsModalVisible = false;

    private void NavigateToScan()
    {
        ScanService.Barcode = Product.Barcode;
        Navigation.NavigateTo($"/scan");
    }
    
    private void ShowReservableProductsModal()
    {
        isReservableProductsModalVisible = true;
    }

    private void HideReservableProductsModal()
    {
        isReservableProductsModalVisible = false;
    }

}