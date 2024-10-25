using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Scan;

public partial class ScanTable : ComponentBase
{
    [Parameter] public required List<ProductDTO> Products { get; set; }
}