using System.Diagnostics;
using System.Web;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Rise.Client.Extensions;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class TableNavbar : ComponentBase
{
    [Parameter] public EventCallback OnShowRegularTable { get; set; }
    [Parameter] public EventCallback OnShowColumnTable { get; set; }
    [CascadingParameter] public IModalService Modal { get; set; } = default!;
    [Parameter] public IList<int>? SelectedCategoriesIds { get; set; }
    [Parameter] public IEnumerable<CategoryDTO>? Categories { get; set; }
    [Parameter] public string? Location { get; set; }
    [Parameter] public IEnumerable<string>? Locations { get; set; }
    [Parameter] public int? MaxInStock { get; set; }
    [Parameter] public int? MinInStock { get; set; }
    [Parameter] public int? MaxOnOrder { get; set; }
    [Parameter] public int? MinOnOrder { get; set; }

    [Parameter] public EventCallback OnFilterApplied { get; set; }

    [Parameter] public Boolean OnlyReservable { get; set; }

    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    private void ShowModal()
    {
        var parameters = new ModalParameters()
            .Add(nameof(FilterModal.Categories), Categories)
            .Add(nameof(FilterModal.SelectedCategoriesIds), SelectedCategoriesIds)
            .Add(nameof(FilterModal.Location), Location)
            .Add(nameof(FilterModal.Locations), Locations)
            .Add(nameof(FilterModal.MaxInStock), MaxInStock)
            .Add(nameof(FilterModal.MinInStock), MinInStock)
            .Add(nameof(FilterModal.MaxOnOrder), MaxOnOrder)
            .Add(nameof(FilterModal.MinOnOrder), MinOnOrder)
            .Add("OnFilterApplied", OnFilterApplied)
            .Add("OnlyReservable", OnlyReservable);
        Modal.Show<FilterModal>("Filters", parameters);
    }

    private ProductRequest.Index GetFilters()
    {
        ProductRequest.Index result = new()
        {
            MaxInStock = MaxInStock,
            MinInStock = MinInStock,
            MaxOnOrder = MaxOnOrder,
            MinOnOrder = MinOnOrder,
            Location = Location,
            CategoryIds = SelectedCategoriesIds,
        };
        Console.WriteLine(Location);
        return result;
    }

    private string? GetCategoryById(int categoryId)
    {
        return Categories?.FirstOrDefault(category => category.Id == categoryId)?.Name;
    }

    private void RemoveFilter(string filter, int? categoryId)
    {
        switch (filter)
        {
            case "Category" when categoryId != null:
                SelectedCategoriesIds?.Remove(categoryId.Value);
                break;
            case "InStock":
                MaxInStock = null;
                MinInStock = null;
                break;
            case "OnOrder":
                MaxOnOrder = null;
                MinOnOrder = null;
                break;
            case "Location":
                Location = null;
                break;
        }
        var request = new
        {
            MaxInStock,
            MinInStock,
            MaxOnOrder,
            MinOnOrder,
            Location,
        };
        var url = "";
        if (OnlyReservable)
        {
            url = "reserve?";
        }
        else
        {
            url = "products?";
        }

        url += request.AsQueryString();

        if (SelectedCategoriesIds != null)
        {
            url = SelectedCategoriesIds.Select(i => i).Aggregate(url, (current, value) => current + $"&Category={value}");
        }
        NavigationManager.NavigateTo(url);
    }

    private string GetFilterRangeString(int? min, int? max)
    {
        var result = string.Empty;
        if (min.HasValue && max.HasValue)
        {
            result = $"{min} - {max}";
        }

        if (min.HasValue && !max.HasValue)
        {
            result = $"> {min}";
        }

        if (!min.HasValue && max.HasValue)
        {
            result = $"< {max}";
        }
        return result;
    }
}