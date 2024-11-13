
using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products.Management;

public partial class Index
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    public ProductDTO SelectedProduct { get; set; } = default!;
    [Inject] private IProductService ProductService { get; set; } = null!;

    private string GetQueryParm(string parmName)
    {
        var uriBuilder = new UriBuilder(NavigationManager.Uri);
        var q = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        return q[parmName] ?? "";
    }


    private string selectedButton = "Wijzigen";
    public void SelectButton(string page)
    {
        selectedButton = page;
    }

    private string GetButtonClass(string buttonName)
    {
        return $"px-3 text-gray-600 rounded-lg {(selectedButton == buttonName ? "bg-gray-300 rounded-lg" : "")}";
    }

    protected override async void OnInitialized()
    {
        SelectedProduct = await ProductService.GetProductByBarcode(GetQueryParm("barcode"));
    }
}