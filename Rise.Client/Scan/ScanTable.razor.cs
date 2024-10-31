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
        [Parameter] public required EventCallback<CartItem> RemoveProduct { get; set; }
        [Inject] public required ICartService CartService { get; set; }
        [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;

        [Inject] private IProductService ProductService { get; set; } = null!;

        // TODO: Put this functin in index.razor.cs
        private async Task CheckoutCart()
        {

            foreach (var cartItem in CartItems)
            {
                var product = await ProductService.GetProductByBarcode(cartItem.Product.Barcode);
                if (cartItem.Quantity > product.QuantityInStock)
                {
                    cartItem.SetInValid();
                }
            }

            if (!CartItems.Any(cartItem => !cartItem.Valid))
            {
                await CartService.CheckoutItems(CartItems);
                CartItems.Clear();
                var storedProducts = await LocalStorage.GetItemAsync<List<CartItem>>("products");
                storedProducts!.Clear();
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