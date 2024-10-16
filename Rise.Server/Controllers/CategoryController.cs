using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Products;

namespace Rise.Server.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IEnumerable<CategoryDTO>> Get()
    {
        var categories = await _categoryService.GetAllCategories();
        return categories;
    }
}