using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.DomainClasses;
using Rise.Persistence;
using Rise.Shared.Cart;
using Rise.Shared.Products;

namespace Rise.Services.Cart;

public class CartService : ICartService
{
    private readonly ApplicationDbContext dbContext;

    public CartService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CheckoutItems(List<ProductDTO> cart)
    {
        Dictionary<ProductDTO, int> checkoutItems = getCheckoutItemsDictionary(cart);

        foreach (var item in checkoutItems)
        {
            var productDTO = item.Key;
            var quantityToCheckout = item.Value;

            var product = await dbContext.Products.Where(p => p.Id == productDTO.Id).FirstOrDefaultAsync();

            if (product != null)
            {
                product.QuantityInStock -= quantityToCheckout;

                // Check minimum
                if (product.QuantityInStock < 0)
                {
                    throw new Exception($"Onvoldoende voorraad voor product {product.Name}");
                }
                dbContext.Products.Update(product);
            }
            else
            {
                // Afhandeling als het product niet gevonden wordt
                throw new Exception($"Product met ID {productDTO.Id} niet gevonden.");
            }
        }
        await dbContext.SaveChangesAsync();
    }

    private Dictionary<ProductDTO, int> getCheckoutItemsDictionary(List<ProductDTO> productList)
    {
        var dict = productList
        .GroupBy(p => p.Barcode)
        .ToDictionary(
            group => group.First(),
            group => group.Count()
        );

        return dict;
    }
}