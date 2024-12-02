using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rise.Shared.Cart;
using Rise.Shared.Products;
using Rise.Shared.Transaction;

namespace Rise.Client.Profile;

public class FakeTransactionService : ITransactionService
{
    private readonly List<TransactionDTO> _transactionHistory;

    public FakeTransactionService()
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
        _transactionHistory =
        [
            new TransactionDTO()
            {
                Id = 1,
                Date = DateTime.MinValue,
                Products =
                [
                    new TransactionItemDTO
                    {
                        Product = product,
                        Quantity = 10
                    }
                ],
                Type = "InScannen",
                UserId = "1"
            }
        ];
    }

    public Task AddTransactionScanOut(List<CartItem> cartItems)
    {
        throw new System.NotImplementedException();
    }

    public Task AddTransactionScanIn(List<CartItem> cartItems)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<TransactionDTO>> GetRecentTransactions()
    {
        return Task.FromResult(_transactionHistory);
    }
}