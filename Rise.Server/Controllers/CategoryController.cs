using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Products;
using Microsoft.AspNetCore.Authorization;
using Minio.DataModel;
using Serilog;
using Rise.Domain.DomainClasses;

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
        Log.Information("Adding new category {category} ✨", category.Name);
        await _categoryService.AddCategory(category);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator, InventoryManager")]
    public async Task Update(int id, [FromBody] CategoryDTO category)
    {
        if (id != category.Id)
        {
            throw new Exception("Id's do not match");
        }
        Log.Information("Updating category with id: {id}, updated to: {category} ✨", id, category.Name);
        await _categoryService.UpdateCategory(category);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator, InventoryManager")]
    public async Task Delete(int id)
    {
        Log.Information("Deleting category with id: {id} ✨", id);
        await _categoryService.DeleteCategory(id);
    }
}