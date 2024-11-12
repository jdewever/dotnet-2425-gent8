using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Products;
using Microsoft.AspNetCore.Authorization;

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
        return await bookingService.GetBookingsByProductIdAsync(productId);
    }
}