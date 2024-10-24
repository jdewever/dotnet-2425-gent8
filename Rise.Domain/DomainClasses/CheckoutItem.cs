namespace Rise.Domain.DomainClasses;

public class CheckoutItem
{
    public Product Product { get; set; }
    public int Quantity { get; set; }

    public CheckoutItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }
}
