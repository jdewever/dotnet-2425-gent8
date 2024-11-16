using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.ProductTable;
public partial class ReservableProductsModal : ComponentBase
{
    protected bool isModalVisible = false;

    protected void ShowModal()
    {
        isModalVisible = true;
    }

    protected void CloseModal()
    {
        isModalVisible = false;
    }
}