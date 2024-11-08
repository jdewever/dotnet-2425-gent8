using Microsoft.AspNetCore.Components;

namespace Rise.Client.Products.Components;

public partial class DeleteButton
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    private void ProductManagement()
    {
        Navigation.NavigateTo("/products/management");
    }
}