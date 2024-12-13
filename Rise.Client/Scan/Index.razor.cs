using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Cart;
using Rise.Shared.Products;
using Microsoft.JSInterop;


namespace Rise.Client.Scan;

public partial class Index : ComponentBase
{
    private List<CartItem> products = new List<CartItem>();
    private bool isMobileView;
    private bool showScanner = true;
    private bool showQuantityModal = false;

    [Inject] private ILocalStorageService localStorage { get; set; } = null!;

    [Inject] private IJSRuntime JS { get; set; } = null!;


    protected override async Task OnParametersSetAsync()
    {
        var storedProducts = await localStorage.GetItemAsync<List<CartItem>>("products");
        if (storedProducts != null)
        {
            products = storedProducts;
        }
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Detecteer mobiele weergave en stel de waarde in
            isMobileView = await JS.InvokeAsync<bool>("detectMobileView");
            StateHasChanged();
        }
    }

    private async Task addProduct((ProductDTO product, int quantity) productInfo)
    {
        if (productInfo.quantity <= 0)
        {
            return;
        }

        var existingProduct = products.Find(p => p.Product.Barcode == productInfo.product.Barcode);
        if (existingProduct != null)
        {
            existingProduct.Quantity += productInfo.quantity;
        }
        else
        {
            products.Add(new CartItem { Product = productInfo.product, Quantity = productInfo.quantity });
        }
        await localStorage.SetItemAsync("products", products);
        showScanner = true;
        await InvokeAsync(StateHasChanged);
    }

    private async Task removeProduct(CartItem item)
    {
        var storedProducts = await localStorage.GetItemAsync<List<CartItem>>("products");
        if (storedProducts == null)
        {
            Console.WriteLine("No items in cart");
            return;
        }

        var existingProduct = storedProducts.Find(p => p.Product.Barcode == item.Product.Barcode);
        if (existingProduct != null)
        {
            storedProducts.Remove(existingProduct);
            await localStorage.SetItemAsync("products", storedProducts);
            products = storedProducts;
            StateHasChanged();
        }
    }

    private int getProductCountByBarcode(string barcode)
    {
        return barcode != "" ? products.Where(p => p.Product.Barcode == barcode).Sum(p => p.Quantity) : 0;
    }



}