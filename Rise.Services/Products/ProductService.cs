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
        IQueryable<ProductDto> query;
        
        if (request.CategoryIds != null && request.CategoryIds.Count != 0)
        {
            IQueryable<ProductDto> temp = 
                from p in dbContext.Products
                from c in p.Categories
                where request.CategoryIds.Contains(c.Id)
                select new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Barcode = p.Barcode,
                    QuantityInStock = p.QuantityInStock,
                    QuantityOnOrder = p.QuantityOnOrder,
                    ClassRoomCode = p.ClassRoomCode,
                    Categories = CategoryEntityToDto(p.Categories)
                };
            query = temp;
        }
        else
        {
            query = dbContext.Products.Select(x => new ProductDto
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
        }

        var products = await query.ToListAsync();

        return products;
    }

    public async Task<IEnumerable<ProductDto>> GetSearchedProducts(string? searchTerm = null)
    {
        IQueryable<ProductDto> query = dbContext.Products
            .Where(x => string.IsNullOrEmpty(searchTerm) ||
                        x.Name.ToLower().Contains(searchTerm.ToLower()) ||
                        x.Barcode.ToLower().Contains(searchTerm.ToLower()))
            .Select(x => new ProductDto
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


    //todo: needs to be in its own class?
    private static List<CategoryDTO> CategoryEntityToDto(List<Category> categories)
    {
        var categoriesDto = new List<CategoryDTO>();
        categories.ForEach(category =>
        {
            CategoryDTO dto = new CategoryDTO
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