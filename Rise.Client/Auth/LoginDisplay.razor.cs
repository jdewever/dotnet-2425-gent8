using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Rise.Shared.User;

namespace Rise.Client.Auth;

public partial class LoginDisplay : ComponentBase
{
    [Inject] public required UserService UserService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    public UserDto? User { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            User = await UserService.GetCurrentUser();
        }
        catch (Exception e)
        {
            NavigationManager.NavigateToLogin("login");
        }
    }

    private void _onclick()
    {
        NavigationManager.NavigateTo("profile");
    }
}