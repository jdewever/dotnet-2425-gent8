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

    [HttpGet]
    public async Task<IEnumerable<ProductDTO>> Get([FromQuery] ProductRequest.Index request)
    {
        var products = await productService.GetAllProducts(request);
        return products;
    }

    [HttpGet("location")]
    public async Task<IEnumerable<string>> GetLocations()
    {
        return await productService.GetAllLocations();
    }

    [HttpGet("barcode")]
    public async Task<ProductDTO> GetProductByBarcode([FromQuery] string? barcode = null)
    {
        return await productService.GetProductByBarcode(barcode);
    }
}
