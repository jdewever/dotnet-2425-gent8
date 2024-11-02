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

    public async Task<int> AddTransactionScanOut()
    {
        var transaction = new UserTransaction(1, "ScanOut");

        dbContext.Transaction.Add(transaction);
        await dbContext.SaveChangesAsync();

        var transactionDB = dbContext.Find<UserTransaction>(transaction.Id) ?? throw new InvalidOperationException("Transaction not found.");

        return transactionDB.Id;
    }
}