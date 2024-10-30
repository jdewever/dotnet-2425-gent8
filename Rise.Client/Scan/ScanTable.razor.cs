using Microsoft.AspNetCore.Components;
using Rise.Shared.Cart;

namespace Rise.Client.Scan
{
    public partial class ScanTable
    {
        [Parameter] public required List<CartItem> CartItems { get; set; }
        [Parameter] public required EventCallback<CartItem> RemoveProduct { get; set; }
        [Inject] public required ICartService CartService { get; set; }

        // TODO: Put this functin in index.razor.cs
        private async Task CheckoutCart()
        {
            await CartService.CheckoutItems(CartItems);
            CartItems.Clear();
            // TODO: How to verify if the checkout was successful?
            /*
            if (response.IsSuccessStatusCode)
            {
                // Handle success (e.g., navigate to a confirmation page, show a success message, etc.)
                // Add toast notification
                Products.Clear();
            }
            else
            {
                // Handle error (e.g., show an error message)
                // Add toast notification
            }
            */
        }
    }
}