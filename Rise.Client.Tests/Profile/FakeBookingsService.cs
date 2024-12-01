using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rise.Shared.Booking;
using Rise.Shared.Products;

namespace Rise.Client.Profile;

public class FakeBookingsService : IBookingService
{
    private readonly IEnumerable<BookingDTO> _fakeBookings;

    public FakeBookingsService()
    {
        var product = new ProductDTO()
        {
            Id = 1,
            Barcode = "barcode",
            Categories =
            [
                new CategoryDTO
                {
                    Id = 1,
                    Name = "category",
                    Products = null
                }
            ],
            Description = "description",
            Name = "name",
            IsReservable = true,
            LowStock = 20,
            ClassRoomCode = "classroomCode",
            QuantityInStock = 100,
            QuantityOnOrder = 10,
            IsHidden = false,
        };
        _fakeBookings =
        [
            new BookingDTO()
            {
                Product = product,
                StartDate = new DateTime(2024, 11, 25, 10, 0, 0),
                EndDate = new DateTime(2024, 11, 25, 12, 0, 0)
            },

            new BookingDTO()
            {
                Product = product,
                StartDate = new DateTime(2024, 11, 26, 14, 0, 0),
                EndDate = new DateTime(2024, 11, 26, 16, 30, 0)
            }
        ];
    }
    public Task<List<BookingDTO>> GetBookingsByProductIdAsync(int productId)
    {
        throw new System.NotImplementedException();
    }

    public Task AddBookingAsync(BookingDTO booking)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<BookingDTO>> GetRecentBookings(bool isHistory)
    {
        return Task.FromResult(_fakeBookings);
    }

    public Task CancelBooking(int id)
    {
        throw new NotImplementedException();
    }
}