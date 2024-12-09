using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Manage.Products
{
    public partial class Index : ComponentBase
    {
        [Parameter] public ManageView<ProductDTO>? ManageViewComponent { get; set; }
        [Inject] private IProductService ProductService { get; set; } = null!;
        [Inject] private IToastService ToastService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        private IEnumerable<ProductDTO>? Products;
        private ProductRequest.Hidden Request = new() { HiddenProducts = true };
        protected override async Task OnInitializedAsync()
        {
            Products = await ProductService.GetHiddenProducts(Request);
        }
        private async Task HandleIsHidden(ProductDTO product)
        {
            await ProductService.ToggleHideProduct(product.Barcode);
            Products = await ProductService.GetHiddenProducts(Request);
            StateHasChanged();
            ToastService.ShowSuccess($"Product {product.Name} is nu zichtbaar!");
        }

        private void NavigateToAddProduct()
        {
            NavigationManager.NavigateTo("/products/add");
        }

        private void HandleEdit(ProductDTO product)
        {
            NavigationManager.NavigateTo($"/products/management/?barcode={product.Barcode}");
        }
    }
}