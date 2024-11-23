namespace Rise.Shared.Transaction;

public interface ITransactionService
{
    Task<int> AddTransactionScanOut();
    Task<int> AddTransactionScanIn();
}