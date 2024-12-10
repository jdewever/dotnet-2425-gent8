using Microsoft.EntityFrameworkCore;
using Rise.Domain.DomainClasses;
using Rise.Persistence;
using Rise.Services.Auth;
using Rise.Shared.Cart;
using Rise.Shared.Products;
using Rise.Shared.Transaction;

namespace Rise.Services.Transaction;

public class TransactionService : ITransactionService
{
    private readonly ApplicationDbContext dbContext;
    private readonly IAuthContextProvider authContextProvider;

    public TransactionService(ApplicationDbContext dbContext, IAuthContextProvider authContextProvider)
    {
        if (authContextProvider.User is null)
            throw new ArgumentNullException($"{nameof(TransactionService)} requires a {nameof(authContextProvider)}");
        this.dbContext = dbContext;
        this.authContextProvider = authContextProvider;
    }

    public async Task AddTransactionScanOut(List<CartItem> cartItems)
    {
        var userid = authContextProvider.User?.Identity?.Name ??
                     throw new InvalidOperationException("User name is null");
        var transaction = new UserTransaction(userid, "ScanOut");
        var transactionItems = new List<TransactionItem>();
        foreach (var cartItem in cartItems)
        {
            var product = await dbContext.Products.Where(p => p.Id == cartItem.Product.Id).FirstOrDefaultAsync() ??
                          throw new InvalidOperationException();
            transactionItems.Add(new TransactionItem(transaction, product, cartItem.Quantity));
        }

        transaction.SetTransactionItems(transactionItems);
        dbContext.Transaction.Add(transaction);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddTransactionScanIn(List<CartItem> cartItems)
    {
        var userid = authContextProvider.User?.Identity?.Name ??
                     throw new InvalidOperationException("User name is null");
        var transaction = new UserTransaction(userid, "ScanIn");
        var transactionItems = new List<TransactionItem>();
        foreach (var cartItem in cartItems)
        {
            var product = await dbContext.Products.Where(p => p.Id == cartItem.Product.Id).FirstOrDefaultAsync() ??
                          throw new InvalidOperationException();
            transactionItems.Add(new TransactionItem(transaction, product, cartItem.Quantity));
        }

        transaction.SetTransactionItems(transactionItems);
        dbContext.Transaction.Add(transaction);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<TransactionDTO>> GetRecentTransactions()
    {
        var userid = authContextProvider.User?.Identity?.Name ??
                     throw new InvalidOperationException("User name is null");
        var query = dbContext.Transaction.Where(t => t.UserId == userid)
            .Include(t => t.TransactionItems)
            .Include(t => t.Products)
            .OrderByDescending(transaction => transaction.CreatedAt);
        return await query.Select(transaction => new TransactionDTO
        {
            Id = transaction.Id,
            Date = transaction.CreatedAt,
            Type = transaction.Type,
            UserId = transaction.UserId,
            Products = transaction.TransactionItems.Select(item => new TransactionItemDTO
            {
                Quantity = item.Quantity,
                Product = new ProductDTO
                {
                    Id = item.Product.Id,
                    Barcode = item.Product.Barcode,
                    Description = item.Product.Description,
                    Name = item.Product.Name,
                    IsReservable = item.Product.IsReservable,
                    LowStock = item.Product.LowStock,
                    ClassRoomCode = item.Product.ClassRoomCode,
                    QuantityInStock = item.Quantity,
                    QuantityOnOrder = item.Quantity,
                    ImageUrl = "/api/proxy/image?url=" + Uri.EscapeDataString(item.Product.ImageUrl),
                    IsHidden = item.Product.IsHidden,
                    Categories = CategoryEntityConverter.CategoryEntityListToDtoList(item.Product.Categories),
                }
            })
        }).ToListAsync();
    }
}