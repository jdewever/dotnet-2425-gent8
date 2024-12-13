using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Products;
using Microsoft.AspNetCore.Authorization;
using Rise.Shared.Booking;
using Serilog;
using BarcodeStandard;
using Rise.Domain.DomainClasses;
using Rise.Persistence.Migrations;

namespace Rise.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService bookingService;

    public BookingController(IBookingService bookingService)
    {
        this.bookingService = bookingService;
    }

    [HttpGet("{productId}")]
    public async Task<IEnumerable<BookingDTO>> GetBookingsByProductId(int productId)
    {
        Log.Information("Getting bookings using productid:{productId} ✨", productId);
        return await bookingService.GetBookingsByProductIdAsync(productId);
    }

    [HttpPost]
    public async Task AddBooking(BookingDTO booking)
    {
        Log.Information("Adding booking:{booking} ✨", booking.Product);
        await bookingService.AddBookingAsync(booking);
    }

    [HttpDelete("{id}")]
    public async Task CancelBooking(int id)
    {
        Log.Information("Canceling booking:{id} ✨", id);
        await bookingService.CancelBooking(id);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IEnumerable<BookingDTO>> GetAllBookings()
    {
        Log.Information("Getting all bookings ✨");
        return await bookingService.GetAllBookings();
    }
}