using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Products;
using Microsoft.AspNetCore.Authorization;
using Rise.Shared.Minio;
using Microsoft.AspNetCore.Components.Forms;
using Serilog;
using Minio.DataModel;
using BarcodeStandard;

namespace Rise.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService productService;
    private readonly IMinioService minio;

    public ProductController(IProductService productService, IMinioService minio)
    {
        this.productService = productService;
        this.minio = minio;
    }

    // get all products
    [HttpGet]
    public async Task<ProductResponse> Get([FromQuery] ProductRequest.Index request)
    {
        Log.Information("Getting products with: CategoryIds: {CategoryIds}, Location: {Location}, MaxInStock: {MaxInStock}, MinInStock: {MinInStock}, MaxOnOrder: {MaxOnOrder}, MinOnOrder: {MinOnOrder}, Searchterm: {Searchterm}, PageNumber: {PageNumber}, PageSize: {PageSize}, OnlyReservable: {OnlyReservable}, IncludeHidden: {IncludeHidden} ✨",
        request.CategoryIds != null ? string.Join(",", request.CategoryIds) : "None",
        request.Location ?? "None",
        request.MaxInStock,
        request.MinInStock,
        request.MaxOnOrder,
        request.MinOnOrder,
        request.Searchterm ?? "None",
        request.PageNumber,
        request.PageSize,
        request.OnlyReservable ?? false,
        request.IncludeHidden ?? false
        );
        var productResponse = await productService.GetAllProducts(request);
        return productResponse;
    }

    // get all hidden products
    [HttpGet("hidden")]
    [Authorize(Roles = "Administrator, Inventory Manager")]
    public async Task<List<ProductDTO>> GetHidden([FromQuery] ProductRequest.Hidden request)
    {
        Log.Information("Getting hidden products✨");
        return await productService.GetHiddenProducts(request);
    }

    // get all locations
    [HttpGet("location")]
    public async Task<IEnumerable<string>> GetLocations()
    {
        Log.Information("Getting all locations✨");
        return await productService.GetAllLocations();
    }

    // get dashboard info
    [HttpGet("dashboard")]
    [Authorize(Roles = "Administrator")]
    public async Task<DashboardDTO> GetDashboardInfo()
    {
        Log.Information("Getting dashboard info✨");
        return await productService.GetDashboardInfo();
    }

    // add a product
    [HttpPost]
    [Authorize(Roles = "Administrator, Inventory Manager")]
    public async Task AddProduct([FromBody] ProductCreationDTO product)
    {
        Log.Information("Adding new product: Name: {Name}, Description: {Description}, Barcode: {Barcode}, QuantityInStock: {QuantityInStock}, QuantityOnOrder: {QuantityOnOrder}, LowStock: {LowStock}, ClassRoomCode: {ClassRoomCode}, IsReservable: {IsReservable}, IsHidden: {IsHidden}, ImageUrl: {ImageUrl}, CategoryOneId: {CategoryOneId}, CategoryTwoId: {CategoryTwoId}, CategoryThreeId: {CategoryThreeId} ✨",
            product.Name,
            product.Description,
            product.Barcode,
            product.QuantityInStock,
            product.QuantityOnOrder,
            product.LowStock,
            product.ClassRoomCode,
            product.IsReservable,
            product.IsHidden,
            product.ImageUrl,
            product.CategoryOneId,
            product.CategoryTwoId,
            product.CategoryThreeId
        );

        await productService.AddProduct(product);
    }


    // get product by barcode
    [HttpGet("{barcode}")]
    public async Task<ProductDTO> GetProductByBarcode(string barcode)
    {
        Log.Information("Get product using barcode: {barcode}✨",barcode);
        return await productService.GetProductByBarcode(barcode);
    }

    // delete a product by barcode
    [HttpDelete("{barcode}")]
    [Authorize(Roles = "Administrator, Inventory Manager")]
    public async Task DeleteProduct(string barcode)
    {
        Log.Information("Deleting product with barcode: {barcode}✨",barcode);
        await productService.DeleteProduct(barcode);
    }

    // hide/unhide a product by barcode
    [HttpPost("{barcode}/hide")]
    [Authorize(Roles = "Administrator, Inventory Manager")]
    public async Task ToggleHideProduct(string barcode)
    {
        Log.Information("Hiding product with barcode:{barcode}✨",barcode);
        await productService.ToggleHideProduct(barcode);
    }

    [HttpPut("{barcode}")]
    [Authorize(Roles = "Administrator, Inventory Manager")]
    public async Task UpdateProduct(string barcode, [FromBody] ProductCreationDTO product)
    {
        Log.Information("Updating product with barcode:{barcode} to: Name: {Name}, Description: {Description}, Barcode: {Barcode}, QuantityInStock: {QuantityInStock}, QuantityOnOrder: {QuantityOnOrder}, LowStock: {LowStock}, ClassRoomCode: {ClassRoomCode}, IsReservable: {IsReservable}, IsHidden: {IsHidden}, ImageUrl: {ImageUrl}, CategoryOneId: {CategoryOneId}, CategoryTwoId: {CategoryTwoId}, CategoryThreeId: {CategoryThreeId} ✨",
            barcode, 
            product.Name,
            product.Description,
            product.Barcode,
            product.QuantityInStock,
            product.QuantityOnOrder,
            product.LowStock,
            product.ClassRoomCode,
            product.IsReservable,
            product.IsHidden,
            product.ImageUrl,
            product.CategoryOneId,
            product.CategoryTwoId,
            product.CategoryThreeId);
        await productService.UpdateProduct(barcode, product);
    }

    [HttpPost("image")]
    [Authorize(Roles = "Administrator, InventoryManager")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        if (!file.ContentType.Contains("image"))
            return BadRequest("File is not an image");

        using var stream = file.OpenReadStream();
        var objectName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        try
        {
            Log.Information("Uploading imagefile: {file}✨", file);
            string fileUrl = await minio.UploadImageAsync(objectName, stream, file.Length, file.ContentType);
            return Ok(fileUrl);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
