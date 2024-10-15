using Microsoft.EntityFrameworkCore;
using Rise.Domain.DomainClasses;
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
            Categories = CategoryEntityToDto(x.Categories)
        });

        var products = await query.ToListAsync();

        return products;
    }

    private List<CategoryDTO> CategoryEntityToDto(List<Category> categories)
    {
        var categoriesDto = new List<CategoryDTO>();
        categories.ForEach(category =>
        {
            CategoryDTO dto =new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Products = null
            };
            categoriesDto.Add(dto);
        });
        return categoriesDto;
    }
}