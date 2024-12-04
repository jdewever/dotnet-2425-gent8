using Rise.Shared.Booking;
using Moq;
using Blazored.Toast.Services;
using System.Collections.Generic;
using Rise.Shared.Products;
using System;
using System.Linq;
using System.Threading.Tasks;
using Rise.Client.Pages.Transaction;

namespace Rise.Client.Pages.Reservation
{
    public class IndexShould : TestContext
    {
        private readonly IBookingService FakeBookingService = new FakeBookingService();
        private readonly IRenderedComponent<Index> Component;
        public IndexShould()
        {
            var mockToastService = new Mock<IToastService>();
            Services.AddSingleton(mockToastService.Object);
            Services.AddSingleton(FakeBookingService);
            Component = RenderComponent<Index>();
        }

        [Fact]
        public void RendersTitleCorrectly()
        {
            var h1 = Component.Find("h1");

            Assert.Equal("Reserveringen", h1.TextContent.Trim());
        }

        [Fact]
        public void RendersToggleButtonCorrectly()
        {
            var togglebutton = Component.Find("span");

            Assert.Equal("Geschiedenis", togglebutton.TextContent.Trim());
        }

        [Fact]
        public void RendersTableHeaderCorrectly()
        {
            var headers = Component.FindAll("th");

            Assert.Equal("Naam", headers[0].TextContent.Trim());
            Assert.Equal("Startdatum", headers[1].TextContent.Trim());
            Assert.Equal("Einddatum", headers[2].TextContent.Trim());
            Assert.Equal("Acties", headers[3].TextContent.Trim());
        }

        [Fact]
        public async Task RendersTableContentWithoutHistoryCorrectly()
        {
            var bookingsCount = await FakeBookingService.GetRecentBookings(false);

            var rows = Component.FindAll("td");

            Assert.Equal(2, bookingsCount.Count());

            Assert.Equal("productTestName", rows[0].TextContent.Trim());
            Assert.Equal(Common.GetRoundedDate(-1, 0).ToString(), rows[1].TextContent.Trim());
            Assert.Equal(Common.GetRoundedDate(-1, 2).ToString(), rows[2].TextContent.Trim());
            Assert.Equal("annuleren", rows[3].TextContent.Trim());

            Assert.Equal("productTestName", rows[4].TextContent.Trim());
            Assert.Equal(Common.GetRoundedDate(-2, 0).ToString(), rows[5].TextContent.Trim());
            Assert.Equal(Common.GetRoundedDate(-2, 2).ToString(), rows[6].TextContent.Trim());
            Assert.Equal("annuleren", rows[7].TextContent.Trim());
        }

        [Fact]
        public async Task RendersTableContentWithHistoryCorrectly()
        {
            var bookingsCount = await FakeBookingService.GetRecentBookings(true);

            var toggleButton = Component.Find("input");
            toggleButton.Click();

            var rows = Component.FindAll("td");

            Assert.Equal(2, bookingsCount.Count());

            Assert.Equal("productTestName", rows[0].TextContent.Trim());
            Assert.Equal(Common.GetRoundedDate(4, 0).ToString(), rows[1].TextContent.Trim());
            Assert.Equal(Common.GetRoundedDate(4, 2).ToString(), rows[2].TextContent.Trim());
            Assert.Equal("", rows[3].TextContent.Trim());

            Assert.Equal("productTestName", rows[4].TextContent.Trim());
            Assert.Equal(Common.GetRoundedDate(15, 0).ToString(), rows[5].TextContent.Trim());
            Assert.Equal(Common.GetRoundedDate(15, 2).ToString(), rows[6].TextContent.Trim());
            Assert.Equal("", rows[7].TextContent.Trim());
        }
    }
}