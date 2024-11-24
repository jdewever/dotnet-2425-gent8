using Microsoft.AspNetCore.Components;
using Rise.Shared.Cart;
using Rise.Client.Products;

namespace Rise.Client.Components.CartTable;
public partial class Table : ComponentBase
{
    [Parameter] public required IEnumerable<CartItem> Items { get; set; }
    [Parameter] public RenderFragment? Columns { get; set; }
    [Parameter] public required EventCallback<CartItem> RemoveProduct { get; set; }

    [Parameter] public EventCallback<int> OnQuantityChanged { get; set; }

    [Inject] private ScanBarcodeService BarcodeService { get; set; } = null!;
    private void OnRemoveProduct(CartItem item)
    {
        RemoveProduct.InvokeAsync(item);
    }
    private void OnRowClick(CartItem cartItem)
    {
        OnRemoveProduct(cartItem);
        BarcodeService.Barcode = cartItem.Product.Barcode;
        OnQuantityChanged.InvokeAsync(cartItem.Quantity);
    }
}