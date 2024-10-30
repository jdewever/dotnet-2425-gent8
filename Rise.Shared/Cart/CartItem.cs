using Rise.Shared.Products;

namespace Rise.Shared.Cart
{
    public class CartItem
    {
        public required ProductDTO Product { get; set; }
        public int Quantity { get; set; }
    }
}