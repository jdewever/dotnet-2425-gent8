using System.Net;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products.Management.Components
{
    public partial class HideProduct : ComponentBase
    {
        [Inject] public required IProductService ProductService { get; set; }
        [Inject] private IToastService ToastService { get; set; } = null!;
        [CascadingParameter] public ProductDTO SelectedProduct { get; set; } = default!;

        private async Task HideProductHandler()
        {
            if (SelectedProduct.IsHidden)
            {
                ToastService.ShowError("Product is al verborgen");
                return;
            }
            await ProductService.ToggleHideProduct(SelectedProduct.Barcode);
            ToastService.ShowSuccess($"Product {SelectedProduct.Name} succesvol verborgen"); // TODO: Catch error
        }
    }
}