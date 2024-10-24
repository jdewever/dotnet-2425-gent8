using Rise.Shared.Products;

public class CheckoutItemDTO()
{
    public required ProductDTO product { get; set; }

    public required int quantity { get; set; }
}