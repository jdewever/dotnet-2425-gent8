using Microsoft.AspNetCore.Components;
using Rise.Client.Auth;
using Rise.Client.Profile;
using Rise.Shared.Transaction;
using Rise.Shared.User;

namespace Rise.Client.Pages;

public partial class Profile : ComponentBase
{
    [Inject] public required IUserService UserService { get; set; }
    [Inject] public required ITransactionService TransactionService { get; set; }
    private bool _loading;
    private UserDto? User { get; set; }
    private IEnumerable<TransactionDto.History> TransactionsList { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        User = await UserService.GetCurrentUser();
        TransactionsList = await TransactionService.GetRecentTransactions();
        _loading = false;
    }
}