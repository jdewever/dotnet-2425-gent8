using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

public class ProductModal : ComponentBase
{
    protected bool isModalVisible = false;
    protected ProductDTO? selectedProduct;

    protected void ShowModal(ProductDTO product)
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