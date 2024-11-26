using Rise.Domain.DomainClasses;

namespace Rise.Shared.Products;

public static class CategoryEntityConverter
{
    public static List<CategoryDTO> CategoryEntityListToDtoList(List<Category> categories)
    {
        var categoriesDto = new List<CategoryDTO>();
        categories.ForEach(category =>
        {
            if (category.IsDeleted) return;
            categoriesDto.Add(new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Products = null
            });
        });
        return categoriesDto;
    }
}