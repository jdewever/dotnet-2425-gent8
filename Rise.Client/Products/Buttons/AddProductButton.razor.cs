using Microsoft.AspNetCore.Components;

namespace Rise.Client.Products.Buttons;

public partial class AddProductButton
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    private void AddProduct()
    {
        Navigation.NavigateTo("/products/add");
    }
    
}