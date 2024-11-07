using Microsoft.AspNetCore.Components;
using Rise.Client.Components.ProductTable;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class ProductColumnView : ProductModal
{
    [Parameter] public required List<ProductDTO> Products { get; set; }

    [Parameter] public Boolean Reserve { get; set; } = false;
}