using Microsoft.EntityFrameworkCore;
using Rise.Domain.DomainClasses;
using Rise.Persistence;
using Rise.Shared.Products;

namespace Rise.Services.Products;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext dbContext;

    public CategoryService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
    {
        IQueryable<CategoryDTO> query = dbContext.Categories.Select(c => new CategoryDTO
        {
            Id = c.Id,
            Name = c.Name,
            Products = null,
        });
        var categories = await query.ToListAsync();
        return categories;
    }
}