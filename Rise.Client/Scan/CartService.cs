using System.Net.Http;
using System.Net.Http.Json;
using Rise.Shared.Cart;
using Rise.Shared.Products;

namespace Rise.Client.Cart
{
    public class CartService : ICartService
    {
        private readonly HttpClient httpClient;
        private const string checkoutEndpoint = "cart/checkout";
        private const string checkinEndpoint = "cart/checkin";

        public CartService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CheckoutItems(List<CartItem> cart)
        {
            await httpClient.PutAsJsonAsync<List<CartItem>>(checkoutEndpoint, cart);
        }

        public async Task CheckInItems(List<CartItem> cart)
        {
            await httpClient.PutAsJsonAsync<List<CartItem>>(checkinEndpoint, cart);
        }
    }
}