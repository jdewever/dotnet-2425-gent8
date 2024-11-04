using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class FilterModal : ComponentBase
{
    [Parameter, EditorRequired] public IEnumerable<CategoryDTO>? Categories { get; set; }
    [Parameter, EditorRequired] public IList<int>? SelectedCategoriesIds { get; set; }
    [Parameter, EditorRequired] public string? Location { get; set; }
    [Parameter, EditorRequired] public IEnumerable<string>? Locations { get; set; }
    [Parameter, EditorRequired] public int? MaxInStock { get; set; }
    [Parameter, EditorRequired] public int? MinInStock { get; set; }
    [Parameter, EditorRequired] public int? MaxOnOrder { get; set; }
    [Parameter, EditorRequired] public int? MinOnOrder { get; set; }
    [Parameter, EditorRequired] public string? StockStatus { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    private int[] _selectedCategoriesIds = [];

    private void Filter()
    {
        if (MaxInStock < MinInStock)
        {
            (MinInStock, MaxInStock) = (MaxInStock, MinInStock);
        }
        if (MaxOnOrder < MinOnOrder)
        {
            (MinOnOrder, MaxOnOrder) = (MaxOnOrder, MinOnOrder);
        }
        var queryString = "?";
        if (!string.IsNullOrEmpty(Location))
        {
            queryString += $"Location={Location}";
        }
        if (MaxInStock is >= 0)
        {
            queryString +=  queryString != "?" ? "&" : "";
            queryString += $"MaxInStock={MaxInStock}";
        }
        if (MinInStock is >= 0)
        {
            queryString +=  queryString != "?" ? "&" : "";
            queryString += $"MinInStock={MinInStock}";
        }
        if (MaxOnOrder is >= 0)
        {
            queryString +=  queryString != "?" ? "&" : "";
            queryString += $"MaxOnOrder={MaxOnOrder}";
        }
        if (MinOnOrder is >= 0)
        {
            queryString +=  queryString != "?" ? "&" : "";
            queryString += $"MinOnOrder={MinOnOrder}";
        }
        if (SelectedCategoriesIds!= null && SelectedCategoriesIds.Count != 0)
        {
            queryString +=  queryString != "?" ? "&" : "";
            _selectedCategoriesIds = SelectedCategoriesIds!.ToArray();
            queryString += string.Join("&", _selectedCategoriesIds.Select(x=>"Category=" + x));
        }
        if (!string.IsNullOrEmpty(StockStatus))
        {
            queryString += queryString != "?" ? "&" : "";
            queryString += $"StockStatus={StockStatus}";
        }
        NavigationManager.NavigateTo($"/products{queryString}");
    }
    private async Task<IEnumerable<CategoryDTO>> SearchCategory(string searchTerm)
    {
        return await Task.FromResult(Categories!.Where(category => category.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)));
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
        return await Task.FromResult(Locations!.Where(location => location.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)));
    }

    private void MaxInStockChanged(ChangeEventArgs args)
    {
        MaxInStock = string.IsNullOrWhiteSpace(args.Value?.ToString()) ? null : int.Parse(args.Value.ToString()!);
    }

    private void MinInStockChanged(ChangeEventArgs args)
    {
        MinInStock = string.IsNullOrWhiteSpace(args.Value?.ToString()) ? null : int.Parse(args.Value.ToString()!);
    }

    private void MaxOnOrderChanged(ChangeEventArgs args)
    {
        MaxOnOrder = string.IsNullOrWhiteSpace(args.Value?.ToString()) ? null : int.Parse(args.Value.ToString()!);
    }

    private void MinOnOrderChanged(ChangeEventArgs args)
    {
        MinOnOrder = string.IsNullOrWhiteSpace(args.Value?.ToString()) ? null : int.Parse(args.Value.ToString()!);
    }
    private void StockStatusChanged(ChangeEventArgs args)
    {
        StockStatus = args.Value?.ToString();

        if (!string.IsNullOrEmpty(StockStatus) && StockStatus != "All")
        {
            if (StockStatus == "InStock")
            {
                MinInStock = 1;
                MaxInStock = null;
            }
            else if (StockStatus == "OutOfStock")
            {
                MinInStock = null;
                MaxInStock = 0;
            }
        }
        else
        {
            MinInStock = null;
            MaxInStock = null;
        }
    }
}