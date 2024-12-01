using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Manage;

public partial class Categories : ComponentBase
{
    private IEnumerable<CategoryDTO>? categories;
    private CategoryDTO? selectedCategory;

    [Parameter] public ManageView<CategoryDTO>? ManageViewComponent { get; set; }
    [Inject] public required ICategoryService CategoryService { get; set; }
    [Inject] private IToastService ToastService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        categories = await CategoryService.GetAllCategories();
    }

    // table actions
    public void ShowAddModal()
    {
        selectedCategory = new CategoryDTO { Id = -1, Name = string.Empty };
    }

    public void ShowEditModal(CategoryDTO category)
    {
        selectedCategory = category;
    }

    public void HideEditModal()
    {
        selectedCategory = null;
    }

    public void ShowDeleteModal(CategoryDTO category)
    {
        ManageViewComponent?.ShowDeleteModal(category, category.Name);
    }

    private void OnDelete(CategoryDTO category)
    {
        CategoryService.DeleteCategory(category.Id);
        // hide the deleted item, instead of reloading all categories
        categories = categories?.Where(c => c.Id != category.Id);
        // todo: catch error
        ToastService.ShowSuccess($"Categorie {category.Name} verwijderd!");
    }

    private async Task OnSave()
    {
        if (selectedCategory is not null)
            if (selectedCategory.Id == -1)
            {
                // new category
                await CategoryService.AddCategory(selectedCategory);
                ToastService.ShowSuccess($"Categorie {selectedCategory.Name} toegevoegd!");
            }
            else
            {
                await CategoryService.UpdateCategory(selectedCategory);
                ToastService.ShowSuccess($"Categorie {selectedCategory.Name} bijgewerkt!");
            }
        categories = await CategoryService.GetAllCategories();
        HideEditModal();
    }
}
