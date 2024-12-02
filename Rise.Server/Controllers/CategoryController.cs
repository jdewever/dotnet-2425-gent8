using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Products;
using Microsoft.AspNetCore.Authorization;

namespace Rise.Server.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
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

    [HttpPost]
    [Authorize(Roles = "Administrator, InventoryManager")]
    public async Task Add([FromBody] CategoryDTO category)
    {
        await _categoryService.AddCategory(category);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator, InventoryManager")]
    public async Task Update(int id, [FromBody] CategoryDTO category)
    {
        // todo: throw bad request error
        if (id != category.Id)
        {
            throw new Exception("Id's do not match");
        }
        await _categoryService.UpdateCategory(category);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator, InventoryManager")]
    public async Task Delete(int id)
    {
        await _categoryService.DeleteCategory(id);
    }
}