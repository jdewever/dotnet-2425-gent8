namespace Rise.Shared.Products;

public interface IBookingService
{
    Task<List<BookingDTO>> GetBookingsByProductIdAsync(int productId);

    Task AddBookingAsync(BookingDTO booking);
}
