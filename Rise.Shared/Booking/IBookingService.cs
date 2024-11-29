using Rise.Shared.Products;

namespace Rise.Shared.Booking;

public interface IBookingService
{
    Task<List<BookingDTO>> GetBookingsByProductIdAsync(int productId);

    Task AddBookingAsync(BookingDTO booking);
    
    Task<IEnumerable<BookingDTO>> GetRecentBookings();

    Task CancelBooking(int id);
}
