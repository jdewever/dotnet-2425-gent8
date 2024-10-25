using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class ProductView : ComponentBase
{
    [Parameter] public required ProductDTO Product { get; set; }
    [Parameter] public string? ImageUrl { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
}