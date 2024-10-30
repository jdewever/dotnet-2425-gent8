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
        var query = dbContext.Products.GroupBy(product => product.ClassRoomCode)
                                      .Select(products => products.Key);
        return await query.ToListAsync();
    }

    public async Task<ProductResponse> GetAllProducts(ProductRequest.Index request)
    {
        IQueryable<ProductDTO> query;

        if (request.CategoryIds != null && request.CategoryIds.Count != 0)
        {
            query = from p in dbContext.Products
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
                        LowStock = p.LowStock,
                        ClassRoomCode = p.ClassRoomCode,
                        Categories = CategoryEntityToDto(p.Categories)
                    };
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
                LowStock = x.LowStock,
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

        int totalCount = await query.CountAsync();
        int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        //paginatie
        int skip = (request.PageNumber - 1) * request.PageSize;
        query = query.Skip(skip).Take(request.PageSize);

        var products = await query.ToListAsync();

        return new ProductResponse
        {
            Products = products.DistinctBy(dto => dto.Id),
            TotalPages = totalPages
        };
    }

    private static List<CategoryDTO> CategoryEntityToDto(List<Category> categories)
    {
        var categoriesDto = new List<CategoryDTO>();
        categories.ForEach(category =>
        {
            categoriesDto.Add(new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Products = null
            });
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
                LowStock = x.LowStock,
                ClassRoomCode = x.ClassRoomCode,
                Categories = CategoryEntityToDto(x.Categories)
            });
        
        var product = await query.FirstOrDefaultAsync();
        return product;
        
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsHavingLowStock()
    {
        IQueryable<ProductDTO> query = dbContext.Products
            .Where(x => x.QuantityInStock + x.QuantityOnOrder < x.LowStock)
            .Select(x => new ProductDTO
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Barcode = x.Barcode,
                QuantityInStock = x.QuantityInStock,
                QuantityOnOrder = x.QuantityOnOrder,
                LowStock = x.LowStock,
                ClassRoomCode = x.ClassRoomCode,
                Categories = CategoryEntityToDto(x.Categories)
            });

        return await query.ToListAsync();
    }
}
