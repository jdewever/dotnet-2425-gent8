using Microsoft.EntityFrameworkCore;
using Rise.Domain.DomainClasses;
using Rise.Persistence;
using Rise.Shared.Products;
using Ardalis.GuardClauses;
using Rise.Shared.Minio;

namespace Rise.Services.Products;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext dbContext;
    private readonly IMinioService minioService;

    public ProductService(ApplicationDbContext dbContext, IMinioService minioService)
    {
        this.dbContext = dbContext;
        this.minioService = minioService;
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
                        ImageUrl = "/api/proxy/image?url=" + Uri.EscapeDataString(p.ImageUrl),
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
                ImageUrl = "/api/proxy/image?url=" + Uri.EscapeDataString(p.ImageUrl),
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
                ImageUrl = "/api/proxy/image?url=" + Uri.EscapeDataString(p.ImageUrl),
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
                ImageUrl = "/api/proxy/image?url=" + Uri.EscapeDataString(p.ImageUrl),
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
            if (product.ImageUrl != null && product.ImageUrl != "")
            {
                await minioService.DeleteImageAsync(product.ImageUrl);
            }
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
        /* TODO: Is dit nog nodig? de barcode service geeft al een unieke barcode terug,
                 maar als 2 mensen tegelijk een product toevoegen kan het zijn dat ze dezelfde barcode krijgen.
                 Daarnaast bestaat de BadRequestException nog niet, Jens hier mee bezig?*/

        // todo: validate categories & product

        // checks if barcode is not already in use and generates a new one if it is.
        // Maybe we should return an error instead?
        /*
        BarcodeService barcodeService = new BarcodeService(dbContext);
        if (!barcodeService.IsValidBarcode(product.Barcode) || await dbContext.Products.AnyAsync(p => p.Barcode == product.Barcode))
        {
            throw new BadRequestException("Barcode is not valid or already in use.", product.Name);
        }
        */

        var newProduct = new Product
        {
            Name = product.Name,
            ClassRoomCode = product.ClassRoomCode,
            Description = product.Description,
            Barcode = product.Barcode,
            QuantityInStock = product.QuantityInStock,
            QuantityOnOrder = product.QuantityOnOrder,
            LowStock = product.LowStock,
            IsReservable = product.IsReservable,
            IsHidden = product.IsHidden,
            ImageUrl = product.ImageUrl,
            Categories = GetCategoriesFromIds(product.GetCategoryIds())
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

    public async Task UpdateProduct(string barcode, ProductCreationDTO product)
    {
        var productToUpdate = await dbContext.Products
            .Include(p => p.Categories)
            .Where(p => p.Barcode == barcode)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Product with barcode '{barcode}' not found.", product.Name);

        // delete old image if new image is uploaded
        if (productToUpdate.ImageUrl != product.ImageUrl)
        {
            if (productToUpdate.ImageUrl != null && productToUpdate.ImageUrl != "")
            {
                await minioService.DeleteImageAsync(productToUpdate.ImageUrl);
            }
        }

        productToUpdate.Name = product.Name;
        productToUpdate.ClassRoomCode = product.ClassRoomCode;
        productToUpdate.Description = product.Description;
        productToUpdate.QuantityInStock = product.QuantityInStock;
        productToUpdate.QuantityOnOrder = product.QuantityOnOrder;
        productToUpdate.LowStock = product.LowStock;
        productToUpdate.IsReservable = product.IsReservable;
        productToUpdate.IsHidden = product.IsHidden;
        productToUpdate.Categories = GetCategoriesFromIds(product.GetCategoryIds());
        productToUpdate.ImageUrl = product.ImageUrl;

        await dbContext.SaveChangesAsync();
    }

    private List<Category> GetCategoriesFromIds(List<int> categoryIds)
    {
        var categories = new List<Category>();
        for (int i = 0; i < categoryIds.Count; i++)
        {
            var category = dbContext.Categories.Find(categoryIds[i]);
            if (category is not null)
            {
                categories.Add(category);
            }
            else
            {
                throw new NotFoundException($"Category with id {categoryIds[i]} not found.", categoryIds[i].ToString());
            }
        }
        return categories;
    }

    public async Task<List<ProductDTO>> GetHiddenProducts(ProductRequest.Hidden request)
    {
        IQueryable<ProductDTO> query = dbContext.Products
            .Where(p => p.IsHidden == request.HiddenProducts)
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
                ImageUrl = "/api/proxy/image?url=" + Uri.EscapeDataString(p.ImageUrl),
                IsHidden = p.IsHidden,
            })
            .OrderBy(p => p.Barcode);

        var products = await query.ToListAsync();
        return products;
    }

    public Task<string> UploadImage(Stream fileStream, string contentType)
    {
        throw new NotImplementedException();
    }
}
