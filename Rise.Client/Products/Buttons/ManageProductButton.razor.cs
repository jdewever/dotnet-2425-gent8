using Microsoft.AspNetCore.Components;

namespace Rise.Client.Products.Buttons;

public partial class ManageProductButton
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    private void ProductManagement()
    {
        Navigation.NavigateTo("/products/management");
    }
}