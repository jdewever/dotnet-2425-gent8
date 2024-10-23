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

    [HttpGet("search")]
    public async Task<IEnumerable<ProductDTO>> GetSearchedProducts([FromQuery] string? searchTerm = null)
    {
        return await productService.GetSearchedProducts(searchTerm);
    }

    [HttpGet("location")]
    public async Task<IEnumerable<string>> GetLocations()
    {
        return await productService.GetAllLocations();
    }
}
