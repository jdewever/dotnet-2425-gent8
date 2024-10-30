using Rise.Domain.DomainClasses;
using Rise.Persistence;
using Rise.Shared.Cart;
using Rise.Shared.Transaction;

namespace Rise.Services.Transaction;

public class TransactionService : ITransactionService
{
    private readonly ApplicationDbContext dbContext;

    public TransactionService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddTransactionScanOut(List<CartItem> transactionItems)
    {
        var transaction = new UserTransaction
        {
            UserId = 1, // TODO: Get user ID from logged in user
            Type = "ScanOut"
        };

        dbContext.Transaction.Add(transaction);
        dbContext.SaveChanges();

        var transId = dbContext.Find<UserTransaction>(transaction.UserId) ?? throw new InvalidOperationException("Transaction not found.");

        foreach (var item in transactionItems)
        {
            var transactionItem = new TransactionItem
            {
                TransactionID = transId.Id,
                ProductID = item.Product.Id,
                Quantity = item.Quantity
            };

            dbContext.TransactionItems.Add(transactionItem);
        }
        await dbContext.SaveChangesAsync();

    }
}