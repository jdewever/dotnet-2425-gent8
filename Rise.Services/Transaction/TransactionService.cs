using Microsoft.EntityFrameworkCore;
using Rise.Domain.DomainClasses;
using Rise.Persistence;
using Rise.Services.Auth;
using Rise.Shared.Cart;
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
        var transaction = new UserTransaction(authContextProvider.User!.Identity.Name, "ScanOut");
        var transactionItems = new List<TransactionItem>();
        foreach (var cartItem in cartItems)
        {
            var product = await dbContext.Products.Where(p => p.Id == cartItem.Product.Id).FirstOrDefaultAsync() ??
                          throw new InvalidOperationException();
            transactionItems.Add(new TransactionItem(transaction, product, cartItem.Quantity));
        }

        transaction.setTransactionItems(transactionItems);
        dbContext.Transaction.Add(transaction);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddTransactionScanIn(List<CartItem> cartItems)
    {
        var transaction = new UserTransaction(authContextProvider.User!.Identity.Name, "ScanIn");
        var transactionItems = new List<TransactionItem>();
        foreach (var cartItem in cartItems)
        {
            var product = await dbContext.Products.Where(p => p.Id == cartItem.Product.Id).FirstOrDefaultAsync() ??
                          throw new InvalidOperationException();
            transactionItems.Add(new TransactionItem(transaction, product, cartItem.Quantity));
        }

        transaction.setTransactionItems(transactionItems);
        dbContext.Transaction.Add(transaction);
        await dbContext.SaveChangesAsync();
    }
}