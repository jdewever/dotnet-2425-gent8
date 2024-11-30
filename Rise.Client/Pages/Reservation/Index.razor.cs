using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;
using Rise.Shared.Booking;
using Blazored.Toast.Services;

namespace Rise.Client.Pages.Reservation;

public partial class Index : ComponentBase
{
    [Parameter] public IEnumerable<BookingDTO>? Reservations { get; set; }
    [Parameter] public RenderFragment? ReservationsTable { get; set; }
    [Inject] public required IBookingService BookingService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    private bool IsHistory { get; set; } = false;

    protected override async Task OnParametersSetAsync()
    {
        Reservations = await BookingService.GetRecentBookings(IsHistory);
    }

    private async Task FilterOutdated()
    {
        IsHistory = !IsHistory;
        Reservations = IsHistory
            ? await BookingService.GetRecentBookings(true)
            : await BookingService.GetRecentBookings(false);
    }

    private async Task HandleCancellation(BookingDTO booking)
    {
        await BookingService.CancelBooking(booking.Id);
        Reservations = Reservations!.Where(r => r.Id != booking.Id);
        ToastService.ShowSuccess("Reservatie geannuleerd");
        StateHasChanged();
    }
}