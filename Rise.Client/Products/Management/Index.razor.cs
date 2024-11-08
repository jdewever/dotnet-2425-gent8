
using Microsoft.AspNetCore.Components;

namespace Rise.Client.Products.Management;

public partial class Index
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    public void SelectButton(string page)
    {
        Navigation.NavigateTo("/product/management" + page);
    }
}