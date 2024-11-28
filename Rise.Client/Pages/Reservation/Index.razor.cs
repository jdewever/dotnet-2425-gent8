using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;
using Rise.Shared.Booking;

namespace Rise.Client.Pages.Reservation;

public partial class Index : ComponentBase
{
    private IEnumerable<BookingDTO>? Reservations;

    [Inject] public required IBookingService BookingService { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        Reservations = await BookingService.GetRecentBookings();
    }

    private void GetProductName(BookingDTO booking)
    {
        Console.WriteLine(booking.Product.Name);
        //return booking.Product.Name;
    }
}