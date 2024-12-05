using Microsoft.AspNetCore.Components;
using Rise.Domain.DomainClasses;
using Rise.Shared.Products;

namespace Rise.Client.Pages.Admindashboard;

public partial class InventoryView : ComponentBase
{
    private DashboardDTO Dashboard = null!;
    private string LowStockProductCount => Dashboard?.LowStockProducts.Count().ToString() ?? "0";
    private string ProductsReserverd => Dashboard?.ProductsReserved.ToString() ?? "0";
    private string ProductsReturning => Dashboard?.ProductsReturning.ToString() ?? "0";
    [Inject] public required IProductService ProductService { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        Dashboard = await ProductService.GetDashboardInfo();
    }
}
