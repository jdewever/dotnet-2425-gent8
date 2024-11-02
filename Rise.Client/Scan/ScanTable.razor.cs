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

        private Boolean selectedButton = true;

        protected override async Task OnParametersSetAsync()
        {
            await checkCartItems();
        }

        private async Task SelectButton(Boolean button)
        {
            selectedButton = button;
            await checkCartItems();
        }

        // TODO: Put this functin in index.razor.cs
        private async Task CheckoutCart()
        {

            await ValidateCartItems();

            if (!CartItems.Any(cartItem => !cartItem.Valid))
            {
                await CartService.CheckoutItems(CartItems);
                CartItems.Clear();
                var storedProducts = await LocalStorage.GetItemAsync<List<CartItem>>("products");
                storedProducts!.Clear();
                await LocalStorage.SetItemAsync("products", storedProducts);
            }
        }

        private async Task CheckInCart()
        {
            await CartService.CheckInItems(CartItems);
            CartItems.Clear();
            var storedProducts = await LocalStorage.GetItemAsync<List<CartItem>>("products");
            storedProducts!.Clear();
            await LocalStorage.SetItemAsync("products", storedProducts);
        }

        private async Task ValidateCartItems()
        {
            foreach (var cartItem in CartItems)
            {
                var product = await ProductService.GetProductByBarcode(cartItem.Product.Barcode);
                cartItem.Valid = cartItem.Quantity <= product.QuantityInStock;
            }
        }

        private void DeVaildateCartItems()
        {
            foreach (var cartItem in CartItems)
            {
                cartItem.Valid = true;
            }
        }

        private async Task checkCartItems()
        {
            if(selectedButton)
            {
                await ValidateCartItems();
            } else
            {
                DeVaildateCartItems();
            }
        }
    }
}