using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class FilterModal : ComponentBase
{
    [Parameter, EditorRequired] public IEnumerable<CategoryDTO>? Categories { get; set; }
    [Parameter, EditorRequired] public IList<int>? SelectedCategoriesIds { get; set; }
    [Parameter, EditorRequired] public string? Location { get; set; }
    [Parameter, EditorRequired] public IEnumerable<string>? Locations { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    private int[] _selectedCategoriesIds = [];

    private void Filter()
    {
        var queryString = "?";
        if (!string.IsNullOrEmpty(Location))
        {
            queryString += $"Location={Location}";
        }
        if (SelectedCategoriesIds!= null && SelectedCategoriesIds.Count != 0)
        {
            queryString +=  queryString != "?" ? "&" : "";
            _selectedCategoriesIds = SelectedCategoriesIds!.ToArray();
            queryString += string.Join("&", _selectedCategoriesIds.Select(x=>"Category=" + x));
        }
        NavigationManager.NavigateTo($"/products{queryString}");
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
    
    private async Task<IEnumerable<string>> SearchLocation(string searchTerm)
    {
        return await Task.FromResult(Locations!.Where(location => location.Contains(searchTerm)));
    }
}