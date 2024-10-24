using Rise.Shared.Products;

namespace Rise.Shared.Cart;

public class CartDTO
{
    public required List<ProductDTO> Products { get; set; }
}