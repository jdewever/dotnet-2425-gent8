using Microsoft.AspNetCore.Components;
using Rise.Client.Scan;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class ProductView : ComponentBase
{
    [Parameter] public required ProductDTO Product { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public EventCallback OnActionButtonClick { get; set; }

    [Parameter] public bool Reserve { get; set; } = false;
}