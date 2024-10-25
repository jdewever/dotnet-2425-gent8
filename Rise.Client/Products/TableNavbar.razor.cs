using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class TableNavbar : ComponentBase
{
    [Parameter] public EventCallback OnShowTable { get; set; }
    [Parameter] public EventCallback OnShowTable2 { get; set; }
    [CascadingParameter] public IModalService Modal { get; set; } = default!;
    [Parameter] public IList<int>? SelectedCategoriesIds { get; set; }
    [Parameter] public IEnumerable<CategoryDTO>? Categories { get; set; }
    [Parameter] public string? Location { get; set; }
    [Parameter] public IEnumerable<string>? Locations { get; set; }
    [Parameter] public int? MaxInStock { get; set; }
    [Parameter] public int? MinInStock { get; set; }
    [Parameter] public int? MaxOnOrder { get; set; }
    [Parameter] public int? MinOnOrder { get; set; }

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
            .Add(nameof(FilterModal.MinOnOrder), MinOnOrder);
        Modal.Show<FilterModal>("Filters", parameters);
    }

}