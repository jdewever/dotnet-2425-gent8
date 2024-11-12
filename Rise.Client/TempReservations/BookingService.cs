using System.Net.Http.Json;
using Rise.Shared.Products;

namespace Rise.Client.Products
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
            return bookings ?? new List<BookingDTO>();
        }
    }
}