using Rise.Shared.Booking;
using Moq;
using System.Collections.Generic;
using Rise.Shared.Products;
using System;
using System.Linq;
using System.Threading.Tasks;
using Rise.Shared.Transaction;

namespace Rise.Client.Pages.Transaction
{
    public class IndexShould : TestContext
    {
        private IEnumerable<TransactionDTO> FakeTransactions = new List<TransactionDTO>();

        [Fact]
        public void RendersTitleCorrectly()
        {
            var mockTransactionService = new Mock<ITransactionService>();
            mockTransactionService
                .Setup(s => s.GetRecentTransactions())
                .ReturnsAsync(GetSampleTransactions().ToList());
            Services.AddSingleton(mockTransactionService.Object);

            var component = RenderComponent<Index>();

            var h1 = component.Find("h1");

            Assert.Equal("Transacties", h1.TextContent.Trim());
        }

        [Fact]
        public void RendersTableHeaderCorrectly()
        {
            var mockTransactionService = new Mock<ITransactionService>();
            mockTransactionService
                .Setup(s => s.GetRecentTransactions())
                .ReturnsAsync(GetSampleTransactions().ToList());
            Services.AddSingleton(mockTransactionService.Object);

            var component = RenderComponent<Index>();

            var headers = component.FindAll("th");

            Assert.Equal("Producten", headers[0].TextContent.Trim());
            Assert.Equal("Hoeveelheden", headers[1].TextContent.Trim());
            Assert.Equal("Datum", headers[2].TextContent.Trim());
            Assert.Equal("Type", headers[3].TextContent.Trim());
        }

        [Fact]
        public async Task RendersTableContentCorrectly()
        {
            var mockTransactionService = new Mock<ITransactionService>();
            mockTransactionService
                .Setup(s => s.GetRecentTransactions())
                .ReturnsAsync(GetSampleTransactions().ToList());
            Services.AddSingleton(mockTransactionService.Object);

            var component = RenderComponent<Index>();

            var transactions = await mockTransactionService.Object.GetRecentTransactions();

            var rows = component.FindAll("td");

            Assert.Equal(3, transactions.Count);

            Assert.Equal("productTestName", rows[0].TextContent.Trim());
            Assert.Equal("4", rows[1].TextContent.Trim());
            Assert.Equal(GetRoundedDate(-1, 2).ToString(), rows[2].TextContent.Trim());
            Assert.Equal("ScanOut", rows[3].TextContent.Trim());

            Assert.Equal("productTestNameproductTestName2", rows[4].TextContent.Trim());
            Assert.Equal("410", rows[5].TextContent.Trim());
            Assert.Equal(GetRoundedDate(-4, 2).ToString(), rows[6].TextContent.Trim());
            Assert.Equal("ScanOut", rows[7].TextContent.Trim());

            Assert.Equal("productTestNameproductTestName2", rows[8].TextContent.Trim());
            Assert.Equal("219", rows[9].TextContent.Trim());
            Assert.Equal(GetRoundedDate(15, 2).ToString(), rows[10].TextContent.Trim());
            Assert.Equal("ScanIn", rows[11].TextContent.Trim());
        }

        private IEnumerable<TransactionDTO> GetSampleTransactions()
        {
            var product1 = new ProductDTO()
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
            var product2 = new ProductDTO()
            {
                Id = 2,
                Barcode = "barcode2",
                Categories =
             [
                 new CategoryDTO
                {
                    Id = 2,
                    Name = "category2",
                    Products = null
                }
             ],
                Description = "description2",
                Name = "productTestName2",
                IsReservable = true,
                LowStock = 200,
                ClassRoomCode = "classroomCode2",
                QuantityInStock = 1000,
                QuantityOnOrder = 100,
                IsHidden = false,
            };
            FakeTransactions =
            [
                new TransactionDTO()
                {
                    Id = 1,
                    Type = "ScanOut",
                    Date = GetRoundedDate(-1, 2),
                    UserId = "auth0|123456",
                    Products =
                    [
                        new()
                        {
                            Product = product1,
                            Quantity = 4
                        }
                    ]
                },
                new TransactionDTO()
                {
                    Id = 2,
                    Type = "ScanOut",
                    Date = GetRoundedDate(-4, 2),
                    UserId = "auth0|123456",
                    Products =
                    [
                        new()
                        {
                            Product = product1,
                            Quantity = 4
                        },
                        new()
                        {
                            Product = product2,
                            Quantity = 10
                        }
                    ]
                },
                new TransactionDTO()
                {
                    Id = 3,
                    Type = "ScanIn",
                    Date = GetRoundedDate(15, 2),
                    UserId = "auth0|123456",
                    Products =
                    [
                        new()
                        {
                            Product = product1,
                            Quantity = 2
                        },
                        new()
                        {
                            Product = product2,
                            Quantity = 19
                        }
                    ]
                },
            ];
            return FakeTransactions;
        }

        public static DateTime GetRoundedDate(int daysDifference, int hoursToAdd)
        {
            var date = DateTime.Now.AddDays(daysDifference).AddHours(hoursToAdd);
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
        }

    }
}