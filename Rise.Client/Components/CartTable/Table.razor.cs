using Microsoft.AspNetCore.Components;
using Rise.Shared.Cart;
using Rise.Client.Products;
using Rise.Client.Scan;

namespace Rise.Client.Components.CartTable;
public partial class Table : ComponentBase
{
    [Parameter] public required IEnumerable<CartItem> Items { get; set; }
    [Parameter] public RenderFragment? Columns { get; set; }
    [Parameter] public required EventCallback<CartItem> RemoveProduct { get; set; }

    [Parameter] public EventCallback<int> OnQuantityChanged { get; set; }

    [Inject] private ScanService ScanService { get; set; } = null!;
    private void OnRemoveProduct(CartItem item)
    {
        RemoveProduct.InvokeAsync(item);
    }
    private void OnRowClick(CartItem cartItem)
    {
        OnRemoveProduct(cartItem);
        ScanService.Barcode = cartItem.Product.Barcode;
        OnQuantityChanged.InvokeAsync(cartItem.Quantity);
    }
}