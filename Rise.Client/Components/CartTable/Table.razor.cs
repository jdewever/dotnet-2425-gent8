using Microsoft.AspNetCore.Components;
using Rise.Shared.Cart;

namespace Rise.Client.Components.CartTable;
public partial class Table : ComponentBase
{
    [Parameter] public required IEnumerable<CartItem> Items { get; set; }
    [Parameter] public RenderFragment? Columns { get; set; }
    [Parameter] public required EventCallback<CartItem> RemoveProduct { get; set; }

    private void OnRemoveProduct(CartItem item)
    {
        RemoveProduct.InvokeAsync(item);
    }
}