using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Services.Auth;
using Rise.Shared.Booking;
using Rise.Shared.Products;

namespace Rise.Services.Booking
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthContextProvider _authContextProvider;

        public BookingService(ApplicationDbContext dbContext, IAuthContextProvider authContextProvider)
        {
            _dbContext = dbContext;
            _authContextProvider = authContextProvider;
        }

        public async Task<List<BookingDTO>> GetBookingsByProductIdAsync(int productId)
        {
            return await _dbContext.Booking
                .Where(b => b.Product.Id == productId)
                .Select(b => new BookingDTO
                {
                    Id = b.Id,
                    Product = new ProductDTO
                    {
                        Id = b.Product.Id,
                        Name = b.Product.Name,
                        Description = b.Product.Description,
                        QuantityOnOrder = b.Product.QuantityOnOrder,
                        QuantityInStock = b.Product.QuantityInStock,
                        Categories = b.Product.Categories.Select(c => new CategoryDTO
                        {
                            Id = c.Id,
                            Name = c.Name
                        }).ToList(),
                        LowStock = b.Product.LowStock,
                        IsReservable = b.Product.IsReservable,
                        IsHidden = b.Product.IsHidden,
                        ClassRoomCode = b.Product.ClassRoomCode,
                        Barcode = b.Product.Barcode,
                    },
                    UserId = b.UserId,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate
                })
                .ToListAsync();
        }

        public async Task AddBookingAsync(BookingDTO booking)
        {
            //TODO: Add validation
            var product = await _dbContext.Products.FindAsync(booking.Product.Id);

            var newBooking =
                new Domain.DomainClasses.Booking(product, booking.UserId, booking.StartDate, booking.EndDate);

            _dbContext.Booking.Add(newBooking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookingDTO>> GetRecentBookings()
        {
            var userid = _authContextProvider.User?.Identity?.Name ??
                         throw new InvalidOperationException("User name is null");
            var bookings = _dbContext.Booking.Where(b => b.UserId == userid).OrderByDescending(b => b.StartDate)
                .Include(b => b.Product).Include(b => b.Product.Categories);
            return await bookings.Select(b => new BookingDTO
            {
                Id = b.Id,
                UserId = b.UserId,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Product = ProductEntityConverter.EntityToDto(b.Product)
            }).ToListAsync();
        }
    }
}