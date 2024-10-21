using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

public class ProductModal : ComponentBase
{
    protected bool isModalVisible = false;
    protected ProductDto? selectedProduct;

    protected void ShowModal(ProductDto product)
    {
        selectedProduct = product;
        isModalVisible = true;
    }

    protected void CloseModal()
    {
        isModalVisible = false;
        selectedProduct = null;
    }
}