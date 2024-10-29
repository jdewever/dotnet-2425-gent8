using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Cart;

namespace Rise.Client.Components.CartTable;
public partial class CartTable : ComponentBase
{
    [Parameter] public required IEnumerable<CartItem> Items { get; set; }
    [Parameter] public RenderFragment? Columns { get; set; }
    [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;

    // TODO: Put this function in index.razor.cs
    // TODO: After removing an item from the cart and adding the same item again, the quantity is added to the previous quantity
    public async Task OnRemove(CartItem item)
    {
        var storedProducts = await LocalStorage.GetItemAsync<List<CartItem>>("products");
        if (storedProducts == null)
        {
            // TODO: Add toast notification
            Console.WriteLine("No items in cart");
            return;
        }
        var existingProduct = storedProducts.Find(p => p.Product.Barcode == item.Product.Barcode);
        if (existingProduct != null)
        {
            storedProducts.Remove(existingProduct);
            await LocalStorage.SetItemAsync("products", storedProducts);
            Items = storedProducts;
            Console.WriteLine("Removed item from cart");
        }
    }

}