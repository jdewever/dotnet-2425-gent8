using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;
using Rise.Shared.Booking;

namespace Rise.Client.Pages.Reservation;

public partial class Index : ComponentBase
{
    private IEnumerable<BookingDTO>? Reservations;

    private bool IsOutdated { get; set; } = true;

    [Inject] public required IBookingService BookingService { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        Reservations = (await BookingService.GetRecentBookings()).OrderBy(r => r.StartDate);
    }

    private async void FilterOutdated()
    {
        Console.WriteLine("voor: " + IsOutdated);
        IsOutdated = !IsOutdated;
        if (IsOutdated)
        {
            Reservations = (await BookingService.GetRecentBookings()).Where(r => r.StartDate < DateTime.Now);
        }
        else
        {
            Reservations = (await BookingService.GetRecentBookings()).Where(r => r.StartDate >= DateTime.Now);
        }
        Console.WriteLine("na: " + IsOutdated);
    }

    private async Task HandleCancellation(BookingDTO booking)
    {
        //await BookingService.CancelBooking(booking.Id);
        Reservations = await BookingService.GetRecentBookings();
    }
}