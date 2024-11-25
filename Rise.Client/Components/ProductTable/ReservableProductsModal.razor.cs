using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;


namespace Rise.Client.Components.ProductTable;
public partial class ReservableProductsModal : ComponentBase
{
  [Parameter] public EventCallback HideModal { get; set; }

    [Parameter] public required ProductDTO Product { get; set; }

    [Inject] private NavigationManager? NavigationManager { get; set; }

    [Inject] private BarcodeService BarcodeService { get; set; } = null!;

    private void NavigateToAgenda()
    {
        BarcodeService.Barcode = Product.Barcode;
        NavigationManager!.NavigateTo("/agenda");
    }
}