using System.Net.Http.Json;
using Rise.Shared.Booking;
using Rise.Shared.Products;

namespace Rise.Client.Reservation
{
    public class BookingService : IBookingService
    {
        private readonly HttpClient _httpClient;

        public BookingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BookingDTO>> GetBookingsByProductIdAsync(int productId)
        {
            var bookings = await _httpClient.GetFromJsonAsync<List<BookingDTO>>($"booking/{productId}");
            return bookings ?? [];
        }

        public async Task AddBookingAsync(BookingDTO booking)
        {
            await _httpClient.PostAsJsonAsync("booking", booking);
        }

        public async Task<IEnumerable<BookingDTO>> GetRecentBookings(bool history)
        {
            var bookings = await _httpClient.GetFromJsonAsync<List<BookingDTO>>($"History/recent/bookings?history={history}");
            return bookings ?? [];
        }

        public async Task CancelBooking(int id)
        {
            await _httpClient.DeleteAsync($"booking/{id}");
        }

        public async Task<IEnumerable<BookingDTO>> GetAllBookings()
        {
            var bookings = await _httpClient.GetFromJsonAsync<List<BookingDTO>>("booking");
            return bookings ?? [];
        }
    }
}