using Rise.Shared.Products;

namespace Rise.Shared.Cart;

public class CartDTO
{
    public required Dictionary<ProductDTO, int> Products { get; set; }
}