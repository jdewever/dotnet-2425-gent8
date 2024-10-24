using Rise.Shared.Products;

namespace Rise.Shared.Cart;

public class CartDTO
{
    public required List<CheckoutItemDTO> checkoutItems { get; set; }
}