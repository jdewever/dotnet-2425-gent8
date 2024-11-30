using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;
using Rise.Shared.Booking;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Web;

namespace Rise.Client.Pages.Reservation;

public partial class Index : ComponentBase
{
    [Inject] public required IBookingService BookingService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    private bool IsHistory { get; set; } = false;
    private IEnumerable<BookingDTO>? Reservations { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Reservations = await BookingService.GetRecentBookings(false);
    }

    private async Task ToggleHistory()
    {
        IsHistory = !IsHistory;
        Reservations = await BookingService.GetRecentBookings(IsHistory);
    }

    private async Task HandleCancellation(BookingDTO booking)
    {
        await BookingService.CancelBooking(booking.Id);
        Reservations = Reservations!.Where(r => r.Id != booking.Id);
        ToastService.ShowSuccess("Reservatie geannuleerd");
        StateHasChanged();
    }
}