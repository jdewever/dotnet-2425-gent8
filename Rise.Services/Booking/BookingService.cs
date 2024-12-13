using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Services.Auth;
using Rise.Shared.Booking;
using Rise.Shared.Exceptions;
using Rise.Shared.Products;
using Rise.Shared.User;

namespace Rise.Services.Booking
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthContextProvider _authContextProvider;
        private IUserService _userService;

        public BookingService(ApplicationDbContext dbContext, IAuthContextProvider authContextProvider, IUserService userService)
        {
            _dbContext = dbContext;
            _authContextProvider = authContextProvider;
            _userService = userService;
        }

        public async Task<List<BookingDTO>> GetBookingsByProductIdAsync(int productId)
        {
            // error if product not found
            _ = await _dbContext.Products.FindAsync(productId) ?? throw new NotFoundException("Product not found");

            return await _dbContext.Booking
                .Where(b => b.Product.Id == productId && b.IsDeleted == false)
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
                        ImageUrl = "/api/proxy/image?url=" + Uri.EscapeDataString(b.Product.ImageUrl),
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
            var userid = _authContextProvider.User?.Identity?.Name ??
                        throw new BadRequestException("User name is null");
            //TODO: Add validation
            var product = await _dbContext.Products.FindAsync(booking.Product.Id) ?? throw new InvalidOperationException("Product not found");

            var newBooking =
                new Domain.DomainClasses.Booking(product, userid, booking.StartDate, booking.EndDate);

            _dbContext.Booking.Add(newBooking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookingDTO>> GetRecentBookings(bool History)
        {
            var userid = _authContextProvider.User?.Identity?.Name ??
                         throw new BadRequestException("User name is null");
            var roles = GetRoles();
            if (roles.Contains("Administrator") || roles.Contains("Inventory Manager"))
            {
                return await GetAllBookings();
            }
            var bookings = _dbContext.Booking.Where(b => b.UserId == userid).OrderByDescending(b => b.StartDate)
                .Include(b => b.Product).Include(b => b.Product.Categories);
            var now = DateTime.UtcNow;
            return await bookings
                .Where(b => b.IsDeleted == false)
                .Where(b => History ? b.EndDate.CompareTo(now) <= 0 : b.EndDate.CompareTo(now) > 0)
                .Select(b => new BookingDTO
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Product = ProductEntityConverter.EntityToDto(b.Product)
                }).ToListAsync();

        }

        public Task CancelBooking(int id)
        {
            var booking = _dbContext.Booking.Find(id) ?? throw new NotFoundException("Booking not found");
            _dbContext.Booking.Remove(booking);
            return _dbContext.SaveChangesAsync();
        }


        public async Task<IEnumerable<BookingDTO>> GetAllBookings()
        {
            var query = _dbContext.Booking.OrderByDescending(b => b.StartDate)
                .OrderBy(b => b.UserId)
                .Include(b => b.Product).Include(b => b.Product.Categories);
            var users = await _userService.GetUsersCached();
            var bookings = await query
                .Where(b => b.IsDeleted == false)
                .Select(b => new BookingDTO
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Product = ProductEntityConverter.EntityToDto(b.Product)
                })
                .ToListAsync();
            foreach (var booking in bookings)
            {
                booking.UserId = users.FirstOrDefault(u => u.UserID == booking.UserId)?.FullName ?? "Unknown";
            }
            return bookings;
        }

        private List<string> GetRoles()
        {
            var rolesClaim = _authContextProvider.User?.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
            var roles = rolesClaim?.Split(',') ?? [];
            return [.. roles];
        }
    }
}