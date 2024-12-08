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
        private IEnumerable<ProductDTO>? Products;
        protected override async Task OnInitializedAsync()
        {
            ProductRequest.Index request = new() { IncludeHidden = true };
            Products = (await ProductService.GetAllProducts(request)).Products.Where(p => p.IsHidden);
        }
        private async Task HandleIsHidden(ProductDTO product)
        {
            await ProductService.ToggleHideProduct(product.Barcode);
            ToastService.ShowSuccess($"Product {product.Name} is nu zichtbaar!");
        }
    }
}