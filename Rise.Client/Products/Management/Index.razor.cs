
using Microsoft.AspNetCore.Components;

namespace Rise.Client.Products.Management;

public partial class Index
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private string selectedButton = "Toevoegen";
    public void SelectButton(string page)
    {
        selectedButton = page;
    }

    private string GetButtonClass(string buttonName)
    {
        return $"px-3 text-gray-600 rounded-lg {(selectedButton == buttonName ? "bg-gray-300 rounded-lg" : "")}";
    }
}