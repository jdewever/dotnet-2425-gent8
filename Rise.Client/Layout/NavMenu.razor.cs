using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Rise.Client.Auth;

namespace Rise.Client.Layout;

public partial class NavMenu : ComponentBase
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject] public required IUserService UserService { get; set; }

    private bool collapseNavMenu = true;
    private string NavHeight => collapseNavMenu ? "sm:h-[calc(100vh-20rem)] sm:min-h-full" : "h-full min-h-full";
    private string? NavMenuCssClass => collapseNavMenu ? "hidden sm:flex" : null;
    
    private string? userRole;

    private List<NavItem> NavItems => new() {
        new NavItem {
            Label = "Dashboard",
            Href = "",
            Icon = "dashboard",
            IsVisible = userRole == "Inventory Manager" || userRole == "Administrator"
        },
        new NavItem { Label = "Voorraadbeheer", Href = "products", Icon = "inventory",
            Children = new[] {
                new NavItem { Label = "Uitlenen", Href = "products" },
                new NavItem { Label = "Reserveren", Href = "reserve" },
            },
        },
        new NavItem { Label = "Scannen", Href = "scan", Icon = "scan" },
        new NavItem { Label = "Reserveringen", Href = "reservations", Icon = "reservation" },
    };

    private void ToggleNavMenu() {
        collapseNavMenu = !collapseNavMenu;
    }
    public void BeginLogOut()
    {
        UserService.ClearUser();
        Navigation.NavigateToLogout("authentication/logout");
    }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            if (user.IsInRole("Administrator"))
            {
                userRole = "Administrator";
            }
            else if (user.IsInRole("Inventory Manager"))
            {
                userRole = "Inventory Manager";
            }
            else
            {
                userRole = "User";
            }
        }
    }

    // only used here, but maybe better in Rise.Shared once linked to users?
    private class NavItem {
        public required string Label { get; set; }
        public required string Href { get; set; }
        public string? Icon { get; set; }
        public NavItem[] Children { get; set; } = Array.Empty<NavItem>();
        public bool IsVisible { get; set; } = true;
    }
}
