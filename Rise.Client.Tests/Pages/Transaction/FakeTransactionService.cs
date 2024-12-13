using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rise.Shared.Cart;
using Rise.Shared.Products;
using Rise.Shared.Transaction;

namespace Rise.Client.Pages.Transaction
{
    public class FakeTransactionService : ITransactionService
    {
        public Task AddTransactionScanIn(List<CartItem> cartItems)
        {
            throw new System.NotImplementedException();
        }

        public Task AddTransactionScanOut(List<CartItem> cartItems)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TransactionDTO>> GetRecentTransactions()
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
                ImageUrl = "https://via.placeholder.com/150",
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
                ImageUrl = "https://via.placeholder.com/150",
            };
            var FakeTransactions = new List<TransactionDTO>
            {
                new()
                {
                    Id = 1,
                    Type = "ScanOut",
                    Date = Common.GetRoundedDate(-1, 2),
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
                new()
                {
                    Id = 2,
                    Type = "ScanOut",
                    Date = Common.GetRoundedDate(-4, 2),
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
                new()
                {
                    Id = 3,
                    Type = "ScanIn",
                    Date = Common.GetRoundedDate(15, 2),
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
            };
            return Task.FromResult(FakeTransactions.ToList());
        }
    }
}