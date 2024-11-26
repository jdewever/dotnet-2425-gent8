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

            var newBooking = new Booking(product, booking.UserId, booking.StartDate, booking.EndDate);

            _dbContext.Booking.Add(newBooking);
            await _dbContext.SaveChangesAsync();
        }
    }
}