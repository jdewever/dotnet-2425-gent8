using Microsoft.AspNetCore.Components;
using Rise.Shared.Cart;

namespace Rise.Client.Components.CartTable;
public partial class CartTable : ComponentBase
{
    [Parameter] public required IEnumerable<CartItem> Items { get; set; }
    [Parameter] public RenderFragment? Columns { get; set; }

    private CartItem? selectedCartItem;

}