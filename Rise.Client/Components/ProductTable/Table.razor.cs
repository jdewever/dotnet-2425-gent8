using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Components.ProductTable;

public partial class Table : ProductModal
{
    [Parameter] public required IEnumerable<ProductDTO> Items { get; set; }
    [Parameter] public RenderFragment? Columns { get; set; }
    [Parameter] public Boolean EnableModal { get; set; } = true;

    [Parameter] public Boolean Dashboard { get; set; } = false;

    [Parameter] public Boolean Reserve {  get; set; } = false;
 
    [Inject] NavigationManager NavigationManager { get; set; }

    [Inject] BarcodeService BarcodeService { get; set; }

    private void OnUitlenenClick(ProductDTO product)
    {
        BarcodeService.Barcode = product.Barcode;
        NavigationManager.NavigateTo("/scan");
    }

    private void OnReserverenClick(ProductDTO product)
    {
        BarcodeService.Barcode = product.Barcode;
        NavigationManager.NavigateTo("/reservations");
    }
}