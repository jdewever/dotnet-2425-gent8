using Microsoft.AspNetCore.Components;
using Rise.Shared.User;

namespace Rise.Client.Auth;

public partial class LoginDisplay : ComponentBase
{
    [Inject] public required UserService UserService { get; set; }
    public UserDto? User { get; set; }

    protected override async Task OnInitializedAsync()
    {
        User = await UserService.GetCurrentUser();
    }
}