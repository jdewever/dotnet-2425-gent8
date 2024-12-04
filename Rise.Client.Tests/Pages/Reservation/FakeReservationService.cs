using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rise.Client.Pages.Transaction;
using Rise.Shared.Booking;
using Rise.Shared.Products;

namespace Rise.Client.Pages.Reservation
{
    public class FakeBookingService : IBookingService
    {
        public Task AddBookingAsync(BookingDTO booking)
        {
            throw new System.NotImplementedException();
        }

        public Task CancelBooking(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<BookingDTO>> GetBookingsByProductIdAsync(int productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<BookingDTO>> GetRecentBookings(bool history)
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
                Name = "productTestName",
                IsReservable = true,
                LowStock = 20,
                ClassRoomCode = "classroomCode",
                QuantityInStock = 100,
                QuantityOnOrder = 10,
                IsHidden = false,
            };
            var FakeBookings = new List<BookingDTO>()
            {
                new()
                {
                    Product = product,
                    StartDate = Common.GetRoundedDate(-1, 0),
                    EndDate = Common.GetRoundedDate(-1, 2)
                },
                new()
                {
                    Product = product,
                    StartDate = Common.GetRoundedDate(-2, 0),
                    EndDate = Common.GetRoundedDate(-2, 2)
                },
                new()
                {
                    Product = product,
                    StartDate = Common.GetRoundedDate(4, 0),
                    EndDate = Common.GetRoundedDate(4, 2)
                },
                new()
                {
                    Product = product,
                    StartDate = Common.GetRoundedDate(15, 0),
                    EndDate = Common.GetRoundedDate(15, 2)
                }
            };
            if (history)
            {
                return Task.FromResult(FakeBookings.GetRange(2, 2).AsEnumerable());
            }
            return Task.FromResult(FakeBookings.GetRange(0, 2).AsEnumerable());
        }
    }
}