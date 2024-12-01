using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rise.Shared.Booking;
using Rise.Shared.Products;
using Xunit.Abstractions;

namespace Rise.Client.Profile;

public class ReservationsShould : TestContext
{
    public ReservationsShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        Services.AddSingleton<IBookingService, FakeBookingsService>();
    }

    [Fact]
    public async Task RendersTableStructureCorrectly()
    {
        var bookings = await Services.GetService<IBookingService>()!.GetRecentBookings(false);

        var component = RenderComponent<Reservations>(parameters => parameters
            .Add(p => p.BookingsList, bookings)
        );

        var caption = component.Find("caption");
        var headers = component.FindAll("th");

        Assert.Equal("Recente reserveringen", caption.TextContent.Trim());
        Assert.Equal("Product", headers[0].TextContent.Trim());
        Assert.Equal("Start", headers[1].TextContent.Trim());
        Assert.Equal("Einde", headers[2].TextContent.Trim());
    }

    [Fact]
    public async Task RendersBookingsCorrectly_WhenBookingsListIsNotEmpty()
    {
        var bookings = await Services.GetService<IBookingService>()!.GetRecentBookings(false);

        var component = RenderComponent<Reservations>(parameters => parameters
            .Add(p => p.BookingsList, bookings)
        );

        var rows = component.FindAll("tbody tr");

        var bookingDtos = bookings.ToList();
        Assert.Equal(bookingDtos.Count(), rows.Count);

        var columns = component.FindAll("tbody tr td");
        var dates = component.FindAll("td p");

        // Validate the first booking
        Assert.Equal(bookingDtos[0].Product.Name, columns[0].TextContent.Trim());
        Assert.Contains(bookingDtos[0].StartDate.ToShortDateString(), dates[0].TextContent); // Start date
        Assert.Contains(bookingDtos[0].StartDate.ToShortTimeString(), dates[1].TextContent);      // Start time
        Assert.Contains(bookingDtos[0].EndDate.ToShortDateString(), dates[2].TextContent); // End date
        Assert.Contains(bookingDtos[0].EndDate.ToShortTimeString(), dates[3].TextContent);      // End time
    }

    [Fact]
    public void ShowsEmptyState_WhenBookingsListIsEmpty()
    {
        // Arrange
        var bookings = new List<BookingDTO>();

        var component = RenderComponent<Reservations>(parameters => parameters
            .Add(p => p.BookingsList, bookings)
        );

        // Act
        var emptyRow = component.Find("tbody tr td");

        // Assert
        Assert.Equal("Geen Reservaties gevonden", emptyRow.TextContent.Trim());
        Assert.Equal("3", emptyRow.GetAttribute("colspan")); // Ensure it spans all columns
    }
}