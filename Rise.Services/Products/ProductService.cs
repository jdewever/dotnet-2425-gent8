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

    public async Task<IEnumerable<ProductDto>> GetAllProducts(ProductRequest.Index request)
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
        //todo: moet herwerkt worden!
        if (request.CategoryIds != null && request.CategoryIds.Count != 0)
        {
            return query.AsEnumerable().Where(p => p.Categories!.Any(c => request.CategoryIds.Contains(c.Id)));
        }
        var products = await query.ToListAsync();

        return products;
    }

    //todo: needs to be in its own class?
    private static List<CategoryDTO> CategoryEntityToDto(List<Category> categories)
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