using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;
using Rise.Shared.Booking;

namespace Rise.Client.Pages.Reservation;

public partial class Index : ComponentBase
{
    [Parameter] public IEnumerable<BookingDTO>? Reservations { get; set; }

    private bool IsOutdated { get; set; } = false;

    [Inject] public required IBookingService BookingService { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        Reservations = (await BookingService.GetRecentBookings()).OrderBy(r => r.StartDate);
    }

    private async Task FilterOutdated()
    {
        IsOutdated = !IsOutdated;
        var recentBookings = await BookingService.GetRecentBookings();
        Reservations = IsOutdated
            ? recentBookings.Where(r => r.StartDate < DateTime.Now)
            : recentBookings.Where(r => r.StartDate >= DateTime.Now);
    }

    private async Task HandleCancellation(BookingDTO booking)
    {
        //await BookingService.CancelBooking(booking.Id);
        Reservations = await BookingService.GetRecentBookings();
    }
}