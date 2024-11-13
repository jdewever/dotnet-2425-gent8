using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.DomainClasses;
using Rise.Persistence;
using Rise.Shared.Products;

namespace Rise.Services.Products
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _dbContext;

        public BookingService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<BookingDTO>> GetBookingsByProductIdAsync(int productId)
        {
            return await _dbContext.Booking
                .Where(b => b.ProductId == productId)
                .Select(b => new BookingDTO
                {
                    Id = b.Id,
                    ProductId = b.ProductId,
                    UserId = b.UserId,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate
                })
                .ToListAsync();
        }

        public async Task AddBookingAsync(BookingDTO booking)
        {
            var newBooking = new Booking(
                booking.ProductId,
                booking.UserId,
                booking.StartDate,
                booking.EndDate
            );

            _dbContext.Booking.Add(newBooking);
            await _dbContext.SaveChangesAsync();
        }
    }
}