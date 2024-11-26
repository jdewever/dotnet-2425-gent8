using Microsoft.AspNetCore.Components;
using Rise.Client.Scan;
using Rise.Shared.Products;


namespace Rise.Client.Products.Components;

public partial class ReservableProductsModal : ComponentBase
{
    [Parameter] public EventCallback HideModal { get; set; }

    [Parameter] public required ProductDTO Product { get; set; }

    [Inject] private NavigationManager? NavigationManager { get; set; }

    [Inject] private ScanService ScanService { get; set; } = null!;

    private void NavigateToAgenda()
    {
        ScanService.Barcode = Product.Barcode;
        NavigationManager!.NavigateTo("/agenda");
    }

    private bool isModalVisible = false;

    private void ShowModal()
    {
        isModalVisible = true;
    }

    private void CloseModal()
    {
        isModalVisible = false;
    }
}