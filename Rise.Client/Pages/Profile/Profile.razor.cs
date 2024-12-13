using Microsoft.AspNetCore.Components;
using Rise.Client.Auth;
using Rise.Shared.User;

namespace Rise.Client.Pages.Profile;

public partial class Profile : ComponentBase
{
    [Inject] public required ILocalUserService UserService { get; set; }
    private UserDTO User { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        User = await UserService.GetCurrentUser();
    }
}