using Microsoft.AspNetCore.Components;
using Rise.Client.Products.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class ProductColumnView : ComponentBase
{
    [Parameter] public required List<ProductDTO> Products { get; set; }
    [Parameter] public bool Reserve { get; set; } = false;

    [Parameter] public required EventCallback<ProductDTO> OnShowModal { get; set; }
    [Parameter] public required EventCallback<ProductDTO> OnProductClick { get; set; }

    private void ShowModal(ProductDTO product)
    {
        OnShowModal.InvokeAsync(product);
    }

    private void ProductClick(ProductDTO product)
    {
        OnProductClick.InvokeAsync(product);
    }
}