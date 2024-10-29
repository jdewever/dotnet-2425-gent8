using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Cart;
using Rise.Shared.Products;

namespace Rise.Client.Scan
{
    public partial class ScanTable
    {
        [Parameter] public required List<ProductDTO> Products { get; set; }
        [Inject] public ICartService CartService { get; set; }

        private List<CartItem> ConvertProductsToCartItems()
        {
            return Products
                .GroupBy(product => product.Id)
                .Select(group => new CartItem
                {
                    Product = group.First(),
                    Quantity = group.Count()
                })
                .ToList();
        }

        private async Task CheckoutCart()
        {
            var cartItems = ConvertProductsToCartItems();
            await CartService.CheckoutItems(cartItems);
            Products.Clear();
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