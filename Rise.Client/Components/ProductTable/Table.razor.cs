using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;
using Rise.Client.Scan;

namespace Rise.Client.Components.ProductTable;

public partial class Table : ProductModal
{
    [Parameter] public required IEnumerable<ProductDTO> Items { get; set; }
    [Parameter] public RenderFragment? Columns { get; set; }
    [Parameter] public bool EnableModal { get; set; } = true;

    [Parameter] public bool Dashboard { get; set; } = false;

    [Parameter] public bool Reserve {  get; set; } = false;
 
    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    [Inject] ScanService BarcodeService { get; set; } = null!;

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