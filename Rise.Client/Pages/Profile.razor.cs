using Microsoft.AspNetCore.Components;
using Rise.Shared.Booking;
using Rise.Shared.Products;
using Rise.Shared.Transaction;
using Rise.Shared.User;

namespace Rise.Client.Pages;

public partial class Profile : ComponentBase
{
    [Inject] public required Auth.IUserService UserService { get; set; }
    [Inject] public required ITransactionService TransactionService { get; set; }
    [Inject] public required IBookingService BookingService { get; set; }
    private bool _loading;
    private UserDto? User { get; set; }
    private IEnumerable<TransactionDTO> TransactionsList { get; set; } = [];
    private IEnumerable<BookingDTO> BookingsList { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        User = await UserService.GetCurrentUser();
        TransactionsList = await TransactionService.GetRecentTransactions();
        BookingsList = await BookingService.GetRecentBookings(false);
        _loading = false;
    }
}