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

    public async Task<IEnumerable<string>> GetAllLocations()
    {
        IQueryable<string> query = dbContext.Products.GroupBy(product => product.ClassRoomCode)
            .Select(products => products.Key);
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProducts(ProductRequest.Index request)
    {
        IQueryable<ProductDTO> query;

        if (request.CategoryIds != null && request.CategoryIds.Count != 0)
        {
            IQueryable<ProductDTO> temp =
                from p in dbContext.Products
                from c in p.Categories
                where request.CategoryIds.Contains(c.Id)
                select new ProductDTO
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
            query = dbContext.Products.Select(x => new ProductDTO
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

        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            query = query.Where(x => x.ClassRoomCode.Equals(request.Location));
        }

        if (request.MaxInStock is not null && request.MaxInStock >= 0)
        {
            query = query.Where(x => x.QuantityInStock <= request.MaxInStock);
        }

        if (request.MinInStock is not null && request.MinInStock >= 0)
        {
            query = query.Where(x => x.QuantityInStock >= request.MinInStock);
        }

        if (request.MaxOnOrder is not null && request.MaxOnOrder >= 0)
        {
            query = query.Where(x => x.QuantityOnOrder <= request.MaxOnOrder);
        }

        if (request.MinOnOrder is not null && request.MinOnOrder >= 0)
        {
            query = query.Where(x => x.QuantityOnOrder >= request.MinOnOrder);
        }

        if (!string.IsNullOrWhiteSpace(request.Searchterm))
        {
            query = query.Where(x => x.Name.ToLower().Contains(request.Searchterm.ToLower()) ||
                                     x.Barcode.ToLower().Contains(request.Searchterm.ToLower()));
        }

        var products = await query.ToListAsync();

        return products.DistinctBy(dto => dto.Id);
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

    public async Task<ProductDTO> GetProductByBarcode(string? barcode = null)
    {
        IQueryable<ProductDTO> query = dbContext.Products
            .Where(x => x.Barcode == barcode)
            .Select(x => new ProductDTO
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
        
        var product = await query.FirstOrDefaultAsync();
        return product;
        
    }
}