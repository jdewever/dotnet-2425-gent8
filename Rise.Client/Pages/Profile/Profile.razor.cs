using Microsoft.AspNetCore.Components;
using Rise.Shared.User;

namespace Rise.Client.Pages.Profile;

public partial class Profile : ComponentBase
{
    [Inject] public required Auth.IUserService UserService { get; set; }
    private UserDto User { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        User = await UserService.GetCurrentUser();
    }
}