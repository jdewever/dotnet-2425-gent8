using Rise.Domain.DomainClasses;
using Rise.Persistence;
using Rise.Shared.Cart;
using Rise.Shared.Transaction;

namespace Rise.Services.Transaction;

public class TransactionItemService : ITransactionItemService
{
    private readonly ApplicationDbContext dbContext;

    public TransactionItemService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddTransactionItems(int transactionId, List<CartItem> transactionItems)
    {
        foreach (var item in transactionItems)
        {
            var transactionItem = new TransactionItem
            {
                TransactionID = transactionId,
                ProductID = item.Product.Id,
                Quantity = item.Quantity
            };

            dbContext.TransactionItems.Add(transactionItem);
        }
        await dbContext.SaveChangesAsync();

    }
}