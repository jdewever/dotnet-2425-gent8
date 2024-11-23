using Rise.Domain.DomainClasses;
using Rise.Persistence;
using Rise.Services.Auth;
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

    public async Task<int> AddTransactionScanOut()
    {
        var transaction = new UserTransaction(authContextProvider.User!.Identity.Name, "ScanOut");

        dbContext.Transaction.Add(transaction);
        await dbContext.SaveChangesAsync();

        var transactionDB = dbContext.Find<UserTransaction>(transaction.Id) ?? throw new InvalidOperationException("Transaction not found.");

        return transactionDB.Id;
    }

    public async Task<int> AddTransactionScanIn()
    {
        var transaction = new UserTransaction(authContextProvider.User!.Identity.Name, "ScanIn");

        dbContext.Transaction.Add(transaction);
        await dbContext.SaveChangesAsync();

        var transactionDB = dbContext.Find<UserTransaction>(transaction.Id) ?? throw new InvalidOperationException("Transaction not found.");

        return transactionDB.Id;
    }
}