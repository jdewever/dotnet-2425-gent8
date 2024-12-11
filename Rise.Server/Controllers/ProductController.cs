using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Products;
using Microsoft.AspNetCore.Authorization;
using Rise.Shared.Minio;
using Microsoft.AspNetCore.Components.Forms;

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
        var productResponse = await productService.GetAllProducts(request);
        return productResponse;
    }

    // get all hidden products
    [HttpGet("hidden")]
    [Authorize(Roles = "Administrator, Inventory Manager")]
    public async Task<List<ProductDTO>> GetHidden([FromQuery] ProductRequest.Hidden request)
    {
        return await productService.GetHiddenProducts(request);
    }

    // get all locations
    [HttpGet("location")]
    public async Task<IEnumerable<string>> GetLocations()
    {
        return await productService.GetAllLocations();
    }

    // get dashboard info
    [HttpGet("dashboard")]
    [Authorize(Roles = "Administrator")]
    public async Task<DashboardDTO> GetDashboardInfo()
    {
        return await productService.GetDashboardInfo();
    }

    // add a product
    [HttpPost]
    [Authorize(Roles = "Administrator, Inventory Manager")]
    public async Task AddProduct([FromBody] ProductCreationDTO product)
    {
        await productService.AddProduct(product);
    }

    // get product by barcode
    [HttpGet("{barcode}")]
    public async Task<ProductDTO> GetProductByBarcode(string barcode)
    {
        return await productService.GetProductByBarcode(barcode);
    }

    // delete a product by barcode
    [HttpDelete("{barcode}")]
    [Authorize(Roles = "Administrator, Inventory Manager")]
    public async Task DeleteProduct(string barcode)
    {
        await productService.DeleteProduct(barcode);
    }

    // hide/unhide a product by barcode
    [HttpPost("{barcode}/hide")]
    [Authorize(Roles = "Administrator, Inventory Manager")]
    public async Task ToggleHideProduct(string barcode)
    {
        await productService.ToggleHideProduct(barcode);
    }

    [HttpPut("{barcode}")]
    [Authorize(Roles = "Administrator, Inventory Manager")]
    public async Task UpdateProduct(string barcode, [FromBody] ProductCreationDTO product)
    {
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
        try {
            string fileUrl = await minio.UploadImageAsync(objectName, stream, file.Length, file.ContentType);
            return Ok(fileUrl);
        } catch (Exception e) {
            return BadRequest(e.Message);
        }
    }
}
