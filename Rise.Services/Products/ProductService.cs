using Microsoft.EntityFrameworkCore;
using Rise.Domain.DomainClasses;
using Rise.Persistence;
using Rise.Shared.Products;
using Rise.Services.Barcodes;
using Rise.Shared.Transaction;
using Ardalis.GuardClauses;

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
                    where !p.IsDeleted
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
                        Categories = CategoryEntityToDto(p.Categories),
                        IsReservable = p.IsReservable,
                        IsHidden = p.IsHidden,
                    };
        }
        else
        {
            query = dbContext.Products.Where(p => !p.IsDeleted).Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Barcode = p.Barcode,
                QuantityInStock = p.QuantityInStock,
                QuantityOnOrder = p.QuantityOnOrder,
                LowStock = p.LowStock,
                ClassRoomCode = p.ClassRoomCode,
                Categories = CategoryEntityToDto(p.Categories),
                IsReservable = p.IsReservable,
                IsHidden = p.IsHidden,
            });
        }

        if (request.IncludeHidden != true)
        {
            query = query.Where(x => !x.IsHidden);
        }

        if (request.OnlyReservable.HasValue)
        {
            if (request.OnlyReservable == true)
            {
                query = query.Where(x => x.IsReservable);
            }
            else
            {
                query = query.Where(x => !x.IsReservable);
            }
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

    // start using this helper
    private static ProductDTO CreateProductDTO(Product product)
    {
        return new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Barcode = product.Barcode,
            QuantityInStock = product.QuantityInStock,
            QuantityOnOrder = product.QuantityOnOrder,
            LowStock = product.LowStock,
            ClassRoomCode = product.ClassRoomCode,
            Categories = CategoryEntityToDto(product.Categories),
            IsReservable = product.IsReservable,
            IsHidden = product.IsHidden,
        };
    }
    private static List<CategoryDTO> CategoryEntityToDto(List<Category> categories)
    {
        var categoriesDto = new List<CategoryDTO>();
        categories.ForEach(category =>
        {
            if (category.IsDeleted) return;
            categoriesDto.Add(new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Products = null
            });
        });
        return categoriesDto;
    }

    public async Task<ProductDTO> GetProductByBarcode(string barcode)
    {
        // todo: check admin / inv mgr to show in case of hidden products
        IQueryable<ProductDTO> query = dbContext.Products
            .Where(p => p.Barcode == barcode && !p.IsDeleted)
            .Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Barcode = p.Barcode,
                QuantityInStock = p.QuantityInStock,
                QuantityOnOrder = p.QuantityOnOrder,
                LowStock = p.LowStock,
                ClassRoomCode = p.ClassRoomCode,
                Categories = CategoryEntityToDto(p.Categories),
                IsReservable = p.IsReservable,
                IsHidden = p.IsHidden,
            });

        var product = await query.FirstOrDefaultAsync();
        if (product == null)
        {
            throw new InvalidOperationException($"Product with barcode '{barcode}' not found.");
        }
        return product;

    }

    public async Task<DashboardDTO> GetDashboardInfo()
    {
        IQueryable<ProductDTO> productlist = dbContext.Products
            .Where(p => p.QuantityInStock + p.QuantityOnOrder < p.LowStock && !p.IsDeleted && p.IsHidden == false)
            .Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Barcode = p.Barcode,
                QuantityInStock = p.QuantityInStock,
                QuantityOnOrder = p.QuantityOnOrder,
                LowStock = p.LowStock,
                ClassRoomCode = p.ClassRoomCode,
                Categories = CategoryEntityToDto(p.Categories),
                IsReservable = p.IsReservable,
                IsHidden = p.IsHidden,
            });

        return new DashboardDTO
        {
            LowStockProducts = await productlist.ToListAsync(),
            ProductsReturning = GetProductsReturning(),
            ProductsReserved = GetProductsReserved()
        };
    }

    public async Task ToggleHideProduct(string barcode)
    {
        var product = await dbContext.Products.Where(p => p.Barcode == barcode && !p.IsDeleted).FirstOrDefaultAsync();
        if (product is not null)
        {
            product.IsHidden = !product.IsHidden;
            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new NotFoundException($"Product with barcode {barcode} not found.", barcode);
        }
    }

    public async Task DeleteProduct(string barcode)
    {
        var product = await dbContext.Products.Where(p => p.Barcode == barcode && !p.IsDeleted).FirstOrDefaultAsync();
        if (product is not null)
        {
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"Product with barcode {barcode} not found.");
        }
    }

    public async Task AddProduct(ProductCreationDTO product)
    {

        var categories = await dbContext.Categories
                                    .Where(c => product.CategoryIds.Contains(c.Id))
                                    .ToListAsync();

        // todo: validate categories & product

        // checks if barcode is not already in use and generates a new one if it is.
        // Maybe we should return an error instead?
        BarcodeService barcodeService = new BarcodeService(dbContext);
        if (!barcodeService.IsValidBarcode(product.Barcode) || await dbContext.Products.AnyAsync(p => p.Barcode == product.Barcode))
        {
            product.Barcode = (await barcodeService.GetNewBarcode()).Barcode;
        }

        var newProduct = new Product
        {
            Name = product.Name,
            Description = product.Description,
            Barcode = product.Barcode,
            QuantityInStock = product.QuantityInStock,
            QuantityOnOrder = product.QuantityOnOrder,
            LowStock = product.LowStock,
            ClassRoomCode = product.ClassRoomCode,
            IsReservable = product.IsReservable,
            IsHidden = product.IsHidden,
            Categories = categories
        };

        dbContext.Products.Add(newProduct);
        await dbContext.SaveChangesAsync();
    }

    private int GetProductsReturning()
    {
        int query = dbContext.Booking
            .Where(b => b.EndDate.Date == DateTime.Today).Count();
        return query;
    }

    private int GetProductsReserved()
    {
        return dbContext.Booking
            .Where(b => b.StartDate.Date <= DateTime.Today && b.EndDate.Date >= DateTime.Today).Count();
    }
}
