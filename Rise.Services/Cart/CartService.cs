using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Shared.Products;
using Rise.Shared.Cart;

namespace Rise.Services.Cart;

public class CartService : ICartService
{
    private readonly ApplicationDbContext dbContext;

    public CartService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CheckoutItems(List<CartItem> cart)
    {

        foreach (var item in cart)
        {
            var productDTO = item.Product;
            var quantityToCheckout = item.Quantity;

            var product = await dbContext.Products.Where(p => p.Id == productDTO.Id).FirstOrDefaultAsync();

            if (product is not null)
            {
                product.QuantityInStock -= quantityToCheckout;

                // Check minimum
                if (product.QuantityInStock < 0)
                {
                    throw new Exception($"Insufficient stock for product {product.Name}");
                }
                dbContext.Products.Update(product);
            }
            else
            {
                // Product not found
                throw new Exception($"Product with ID {productDTO.Id} not found.");
            }
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task CheckInItems(List<CartItem> cart)
    {
        foreach (var item in cart)
        {
            var productDTO = item.Product;
            var quantityToCheckIn = item.Quantity;

            var product = await dbContext.Products.Where(p => p.Id == productDTO.Id).FirstOrDefaultAsync();

            if (product is not null)
            {
                product.QuantityInStock += quantityToCheckIn;
                dbContext.Products.Update(product);
            }
            else
            {
                // Product not found
                throw new Exception($"Product with ID {productDTO.Id} not found.");
            }
        }
        await dbContext.SaveChangesAsync();
    }
}