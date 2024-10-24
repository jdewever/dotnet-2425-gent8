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

    public async Task CheckoutItems(CartDTO cart)
    {
        foreach (ProductDTO item in cart.Products)
        {
            IQueryable<string> query = dbContext.Products.Where();
            await query.ExecuteUpdate();
        }
    }
}