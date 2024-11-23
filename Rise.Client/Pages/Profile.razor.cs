using Microsoft.AspNetCore.Components;
using Rise.Client.Auth;
using Rise.Shared.Transaction;
using Rise.Shared.User;

namespace Rise.Client.Pages;

public partial class Profile : ComponentBase
{
    [Inject] public required UserService UserService { get; set; }
    private bool _loading;
    private UserDto? User { get; set; }
    private IEnumerable<TransactionDto> TransactionsList { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        User = await UserService.GetCurrentUser();
        _loading = false;
    }
}