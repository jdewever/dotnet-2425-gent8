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

    // get all hidden products
    [HttpGet("hidden")]
    [Authorize(Roles = "Administrator")]
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
    [Authorize(Roles = "Administrator, InventoryManager")]
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

    [HttpPut("{barcode}")]
    [Authorize(Roles = "Administrator, InventoryManager")]
    public async Task UpdateProduct(string barcode, [FromBody] ProductCreationDTO product)
    {
        await productService.UpdateProduct(barcode, product);
    }
}
