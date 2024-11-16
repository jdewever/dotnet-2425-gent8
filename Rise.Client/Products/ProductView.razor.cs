using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class ProductView : ComponentBase
{
    [Parameter] public required ProductDTO Product { get; set; }
    [Parameter] public string? ImageUrl { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public Boolean Reserve { get; set; } = false;

    [Inject] private BarcodeService BarcodeService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private bool isReservableProductsModalVisible = false;

    private void NavigateToScan()
    {
        BarcodeService.Barcode = Product.Barcode;
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