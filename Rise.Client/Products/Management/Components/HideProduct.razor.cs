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
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        private async void HideProductHandler()
        {
            await ProductService.ToggleHideProduct(SelectedProduct.Barcode);
            ToastService.ShowSuccess($"Product {SelectedProduct.Name} succesvol verborgen"); // TODO: Catch error
            NavigationManager.NavigateTo("/products");
        }
    }
}