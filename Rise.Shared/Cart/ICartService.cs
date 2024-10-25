using Rise.Shared.Products;

namespace Rise.Shared.Cart;

public interface ICartService
{
    Task CheckoutItems(List<ProductDTO> cart);
}
