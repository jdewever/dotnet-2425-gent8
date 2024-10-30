using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Cart;
using Rise.Shared.Products;

namespace Rise.Client.Scan
{
    public partial class ScanTable
    {
        [Parameter] public required List<CartItem> CartItems { get; set; }
        [Inject] public required ICartService CartService { get; set; }
        [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;

        // TODO: Put this functin in index.razor.cs
        private async Task CheckoutCart()
        {
            await CartService.CheckoutItems(CartItems);
            CartItems.Clear();
            var storedProducts = await LocalStorage.GetItemAsync<List<CartItem>>("products");
            if (storedProducts == null)
            {
                // TODO: Add toast notification
                Console.WriteLine("No items in cart");
                return;
            }
            else
            {
                storedProducts.Clear();
                await LocalStorage.SetItemAsync("products", storedProducts);
            }
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