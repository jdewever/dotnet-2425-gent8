using System.Net;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products.Management.Components
{
    public partial class HideProduct : ComponentBase
    {
        [Inject]
        public required IProductService ProductService { get; set; }
        [Inject] private IToastService ToastService { get; set; } = null!;

        private async Task HideProductHandler()
        {
            await ProductService.ToggleHideProduct("123456789012"); // TODO: Implement barcode input
            ToastService.ShowSuccess("Producten succesvol uitgescand"); // TODO: Catch error
        }
    }
}