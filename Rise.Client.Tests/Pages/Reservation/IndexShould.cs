using Xunit.Abstractions;
using Rise.Shared.Booking;
using Rise.Client.Profile;
using Moq;
using Blazored.Toast.Services;
using System.Collections.Generic;
using Rise.Shared.Products;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rise.Client.Pages.Reservation
{
    public class IndexShould : TestContext
    {
        private IEnumerable<BookingDTO> _fakeBookings = [];
        public IndexShould()
        {
            var mockToastService = new Mock<IToastService>();
            Services.AddSingleton(mockToastService.Object);
        }

        [Fact]
        public void RendersTitleCorrectly()
        {
            var mockBookingService = new Mock<IBookingService>();
            mockBookingService
                .Setup(s => s.GetRecentBookings(false))
                .ReturnsAsync(GetSampleBookings(false));
            Services.AddSingleton(mockBookingService.Object);

            var component = RenderComponent<Index>();

            var h1 = component.Find("h1");

            Assert.Equal("Reserveringen", h1.TextContent.Trim());
        }

        [Fact]
        public void RendersToggleButtonCorrectly()
        {
            var mockBookingService = new Mock<IBookingService>();
            mockBookingService
                .Setup(s => s.GetRecentBookings(false))
                .ReturnsAsync(GetSampleBookings(false));
            Services.AddSingleton(mockBookingService.Object);

            var component = RenderComponent<Index>();

            var togglebutton = component.Find("span");

            Assert.Equal("Geschiedenis", togglebutton.TextContent.Trim());
        }

        [Fact]
        public void RendersTableHeaderCorrectly()
        {
            var mockBookingService = new Mock<IBookingService>();
            mockBookingService
                .Setup(s => s.GetRecentBookings(false))
                .ReturnsAsync(GetSampleBookings(false));
            Services.AddSingleton(mockBookingService.Object);

            var component = RenderComponent<Index>();

            var headers = component.FindAll("th");

            Assert.Equal("Naam", headers[0].TextContent.Trim());
            Assert.Equal("Startdatum", headers[1].TextContent.Trim());
            Assert.Equal("Einddatum", headers[2].TextContent.Trim());
            Assert.Equal("Acties", headers[3].TextContent.Trim());
        }

        [Fact]
        public async Task RendersTableContentWithoutHistoryCorrectly()
        {
            var mockBookingService = new Mock<IBookingService>();
            mockBookingService
                .Setup(s => s.GetRecentBookings(false))
                .ReturnsAsync(GetSampleBookings(false));
            Services.AddSingleton(mockBookingService.Object);

            var component = RenderComponent<Index>();

            var bookingsCount = await mockBookingService.Object.GetRecentBookings(false);

            var rows = component.FindAll("td");

            Assert.Equal(2, bookingsCount.Count());

            Assert.Equal("productTestName", rows[0].TextContent.Trim());
            Assert.Equal(DateTime.Now.AddDays(-1).ToString(), rows[1].TextContent.Trim());
            Assert.Equal(DateTime.Now.AddDays(-1).AddHours(2).ToString(), rows[2].TextContent.Trim());
            Assert.Equal("annuleren", rows[3].TextContent.Trim());

            Assert.Equal("productTestName", rows[4].TextContent.Trim());
            Assert.Equal(DateTime.Now.AddDays(-2).ToString(), rows[5].TextContent.Trim());
            Assert.Equal(DateTime.Now.AddDays(-2).AddHours(2).ToString(), rows[6].TextContent.Trim());
            Assert.Equal("annuleren", rows[7].TextContent.Trim());

            Assert.Equal(8, rows.Count);
        }

        [Fact]
        public async Task RendersTableContentWithHistoryCorrectly()
        {
            var mockBookingService = new Mock<IBookingService>();
            mockBookingService
                .Setup(s => s.GetRecentBookings(true))
                .ReturnsAsync(GetSampleBookings(true));
            Services.AddSingleton(mockBookingService.Object);

            var component = RenderComponent<Index>();

            var bookingsCount = await mockBookingService.Object.GetRecentBookings(true);

            var toggleButton = component.Find("input");
            toggleButton.Click();

            var rows = component.FindAll("td");

            Assert.Equal(2, bookingsCount.Count());

            Assert.Equal("productTestName", rows[0].TextContent.Trim());
            Assert.Equal(DateTime.Now.AddDays(4).ToString(), rows[1].TextContent.Trim());
            Assert.Equal(DateTime.Now.AddDays(4).AddHours(2).ToString(), rows[2].TextContent.Trim());
            Assert.Equal("", rows[3].TextContent.Trim());

            Assert.Equal("productTestName", rows[4].TextContent.Trim());
            Assert.Equal(DateTime.Now.AddDays(15).ToString(), rows[5].TextContent.Trim());
            Assert.Equal(DateTime.Now.AddDays(15).AddHours(2).ToString(), rows[6].TextContent.Trim());
            Assert.Equal("", rows[7].TextContent.Trim());
        }

        private IEnumerable<BookingDTO> GetSampleBookings(bool history)
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
            _fakeBookings =
            [
                new BookingDTO()
                {
                    Product = product,
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now.AddDays(-1).AddHours(2)
                },
                new BookingDTO()
                {
                    Product = product,
                    StartDate = DateTime.Now.AddDays(-2),
                    EndDate = DateTime.Now.AddDays(-2).AddHours(2)
                },
                new BookingDTO()
                {
                    Product = product,
                    StartDate = DateTime.Now.AddDays(4),
                    EndDate = DateTime.Now.AddDays(4).AddHours(2)
                },
                new BookingDTO()
                {
                    Product = product,
                    StartDate = DateTime.Now.AddDays(15),
                    EndDate = DateTime.Now.AddDays(15).AddHours(2)
                }
            ];
            if (history)
            {
                return _fakeBookings.ToList().GetRange(2, 2);
            }
            return _fakeBookings.ToList().GetRange(0, 2);
        }
    }
}