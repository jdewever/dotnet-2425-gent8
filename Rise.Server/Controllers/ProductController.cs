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
    public async Task<IEnumerable<ProductDto>> Get()
    {
        var products = await productService.GetAllProducts();
        return products;
    }

    [HttpGet("search")]
    public async Task<IEnumerable<ProductDto>> GetSearchedProducts([FromQuery] string? searchTerm = null)
    {
        return await productService.GetSearchedProducts(searchTerm);
    }
}
