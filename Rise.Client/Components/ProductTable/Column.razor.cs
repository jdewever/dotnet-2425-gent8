using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Components.ProductTable;

public partial class Column : ComponentBase
{

    [Parameter] public required string Name { get; set; }
    [Parameter] public required string Label { get; set; }
    [CascadingParameter] public ProductDTO? Item { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
} 