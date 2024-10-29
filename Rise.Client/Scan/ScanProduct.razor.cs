using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Scan;

public partial class ScanProduct : ComponentBase
{
    [Parameter] public ProductDTO? Product { get; set; }
}