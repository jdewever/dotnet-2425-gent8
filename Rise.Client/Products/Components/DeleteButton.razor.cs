using Microsoft.AspNetCore.Components;

partial class DeleteButton
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    private Task ProductManagement()
    {
        Navigation.NavigateTo("/products");
    }
}