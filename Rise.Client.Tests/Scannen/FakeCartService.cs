using System.Collections.Generic;
using System.Threading.Tasks;
using Rise.Shared.Cart;

namespace Rise.Client.Scannen
{
    public class FakeCartService : ICartService
    {
        public Task CheckInItems(List<CartItem> cart)
        {
            throw new System.NotImplementedException();
        }

        public Task CheckoutItems(List<CartItem> cart)
        {
            cart.Clear();
            return Task.CompletedTask;
        }
    }
}