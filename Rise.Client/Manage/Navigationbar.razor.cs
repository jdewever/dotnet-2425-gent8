using Microsoft.AspNetCore.Components;

namespace Rise.Client.Manage;

public partial class Navigationbar : ComponentBase
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private List<NavItem> Items => new()
    {
        new NavItem { Label = "Categorieën", Href = "categories" },
/*        new NavItem { Label = "Locaties", Href = "locations" },*/
        new NavItem { Label = "Gebruikers", Href = "users" },
        new NavItem { Label = "Verborgen Producten", Href = "hiddenproducts" },
    };

    public void OpenPage(string path)
    {
        Navigation.NavigateTo($"manage/{path}");
    }

    public string GetButtonClass(string path)
    {
        return $"px-3 text-gray-600 rounded-lg {(Navigation.ToBaseRelativePath(Navigation.Uri) == $"manage/{path}" ? "bg-gray-300 rounded-lg" : "")}";
    }

    private class NavItem
    {
        public required string Label { get; set; } = default!;
        public required string Href { get; set; } = default!;
    }
}
