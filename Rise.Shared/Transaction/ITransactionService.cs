using Rise.Shared.Cart;

namespace Rise.Shared.Transaction;

public interface ITransactionService
{
    Task<int> AddTransactionScanOut();
}