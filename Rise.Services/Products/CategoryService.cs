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
        IQueryable<CategoryDTO> query = dbContext.Categories.Where(c => !c.IsDeleted).Select(c => new CategoryDTO
        {
            Id = c.Id,
            Name = c.Name,
            Products = null,
        });
        var categories = await query.ToListAsync();
        return categories;
    }

    public async Task AddCategory(CategoryDTO category)
    {
        var newCategory = new Category
        {
            Name = category.Name,
        };
        dbContext.Categories.Add(newCategory);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateCategory(CategoryDTO category)
    {
        var existingCategory = await dbContext.Categories
            .Where(c => !c.IsDeleted)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == category.Id);

        if (existingCategory is not null)
        {
            existingCategory.Name = category.Name;
            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"Category with id {category.Id} not found");
        }
    }

    public async Task DeleteCategory(int id)
    {
        var category = await dbContext.Categories
            .Where(c => !c.IsDeleted)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is not null)
        {
            foreach (var product in category.Products)
            {
                product.Categories.Remove(category);
            }

            // marks the Category as deleted with IsDeleted = true, due to the soft delete pattern
            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"Category with id {id} not found");
        }
    }
}