using System.Net.Http;
using System.Net.Http.Json;
using Rise.Shared.Cart;
using Rise.Shared.Products;

namespace Rise.Client.Cart
{
    public class CartService : ICartService
    {
        private readonly HttpClient httpClient;
        private const string endpoint = "cart";

        public CartService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CheckoutItems(List<CartItem> cart)
        {
            await httpClient.PutAsJsonAsync<List<CartItem>>(endpoint, cart);
        }
    }
}