using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Products;
using Microsoft.AspNetCore.Authorization;

namespace Rise.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService productService;

    public ProductController(IProductService productService)
    {
        this.productService = productService;
    }

    // get all products
    [HttpGet]
    public async Task<ProductResponse> Get([FromQuery] ProductRequest.Index request)
    {
        var productResponse = await productService.GetAllProducts(request);
        return productResponse;
    }

    // get all locations
    [HttpGet("location")]
    public async Task<IEnumerable<string>> GetLocations()
    {
        return await productService.GetAllLocations();
    }

    // get products with low stock
    [HttpGet("lowstock")]
    public async Task<IEnumerable<ProductDTO>> GetProductsHavingLowStock()
    {
        return await productService.GetProductsHavingLowStock();
    }

    // add a product
    [HttpPost]
    [Authorize(Roles = "Administrator, InventoryManager")]
    public async Task AddProduct([FromBody] ProductCreationDTO product)
    {
        await productService.AddProduct(product);
    }

    // get a new barcode that's not in use
    [HttpGet("barcode")]
    [Authorize(Roles = "Administrator, InventoryManager")]
    public async Task<BarcodeResponse> GetNewBarcode()
    {
        return await productService.GetNewBarcode();
    }

    // get the image of a product by barcode
    [HttpGet("{barcode}/image")]
    [Authorize(Roles = "Administrator, InventoryManager")]
    public async Task<ActionResult> GetBarcodeImage(string barcode)
    {
        var imageBase64 = await productService.GetBarcodeImage(barcode);
        var imageBytes = Convert.FromBase64String(imageBase64);
        return File(imageBytes, "image/png");
    }

    // get product by barcode
    [HttpGet("{barcode}")]
    public async Task<ProductDTO> GetProductByBarcode(string barcode)
    {
        return await productService.GetProductByBarcode(barcode);
    }
    
    // delete a product by barcode
    [HttpDelete("{barcode}")]
    [Authorize(Roles = "Administrator, InventoryManager")]
    public async Task DeleteProduct(string barcode)
    {
        await productService.DeleteProduct(barcode);
    }

    // hide/unhide a product by barcode
    [HttpPost("{barcode}/hide")]
    [Authorize(Roles = "Administrator, InventoryManager")]
    public async Task ToggleHideProduct(string barcode)
    {
        await productService.ToggleHideProduct(barcode);
    }
}
