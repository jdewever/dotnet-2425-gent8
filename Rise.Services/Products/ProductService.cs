using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Shared.Products;

namespace Rise.Services.Products;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext dbContext;

    public ProductService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        IQueryable<ProductDto> query = dbContext.Products.Select(x => new ProductDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Barcode = x.Barcode,
            QuantityInStock = x.QuantityInStock,
            QuantityOnOrder = x.QuantityOnOrder,
            ClassRoomCode = x.ClassRoomCode,
        });

        var products = await query.ToListAsync();

        return products;
    }
}