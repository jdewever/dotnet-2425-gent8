using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class FilterModal : ComponentBase
{
    [Parameter, EditorRequired] public IEnumerable<CategoryDTO>? Categories { get; set; }
    [Parameter, EditorRequired] public IList<int>? SelectedCategoriesIds { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    private int[] _selectedCategoriesIds = [];

    private void Filter()
    {
        if (SelectedCategoriesIds!= null && SelectedCategoriesIds.Count != 0)
        {
            _selectedCategoriesIds = SelectedCategoriesIds!.ToArray();
            var queryString = string.Join("&", _selectedCategoriesIds.Select(x=>"SelectedCategory=" + x));
            NavigationManager.NavigateTo($"/products?{queryString}");
        }
        else
        {
            NavigationManager.NavigateTo($"/products");
        }
    }
    private async Task<IEnumerable<CategoryDTO>> SearchCategory(string searchTerm)
    {
        return await Task.FromResult(Categories!.Where(category => category.Name.Contains(searchTerm)));
    }
    private int ConvertMethod(CategoryDTO arg)
    {
        return arg.Id;
    }

    private CategoryDTO GetCategory(int categoryId)
    {
        return Categories!.FirstOrDefault(category => category.Id == categoryId)!;
    }
}