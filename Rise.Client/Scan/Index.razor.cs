using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Scan;

public partial class Index : ComponentBase
{
    private List<ProductDTO> products = new List<ProductDTO>();
    private ProductDTO? selectedProduct = null;
}