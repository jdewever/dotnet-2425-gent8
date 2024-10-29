using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Cart;
using Rise.Shared.Products;

namespace Rise.Client.Scan;

public partial class Index : ComponentBase
{
    private List<CartItem> products = new List<CartItem>();
    private ProductDTO? selectedProduct = null;

    [Inject] private ILocalStorageService localStorage { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        var storedProducts = await localStorage.GetItemAsync<List<CartItem>>("products");
        if (storedProducts != null)
        {
            products = storedProducts;
        }
    }

    private async Task addProduct((ProductDTO product, int quantity) productInfo)
    {
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
    }

    private async Task removeCartItem(CartItem product)
    {
        var existingProduct = products.Find(p => p == product);
        if (existingProduct != null)
        {
            products.Remove(existingProduct);
            await localStorage.SetItemAsync("products", products);
        }
    }

    private int getProductCountByBarcode(string barcode)
    {
        return barcode != "" ? products.Count(p => p.Product.Barcode == barcode) : 0;
    }

}