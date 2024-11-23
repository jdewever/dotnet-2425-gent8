using Microsoft.AspNetCore.Components;
using Rise.Client.Auth;
using Rise.Shared.User;

namespace Rise.Client.Pages;

public partial class Profile : ComponentBase
{
    [Inject] public required UserService UserService { get; set; }
    private UserDto? User { get; set; }

    protected override async Task OnInitializedAsync()
    {
        User = await UserService.GetCurrentUser();
    }
}