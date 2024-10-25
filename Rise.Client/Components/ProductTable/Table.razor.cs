using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Components.ProductTable;

public partial class Table : ProductModal
{
    [Parameter] public required IEnumerable<ProductDTO> Items { get; set; }
    [Parameter] public RenderFragment? Columns { get; set; }
    [Parameter] public Boolean EnableModal { get; set; } = true;
}