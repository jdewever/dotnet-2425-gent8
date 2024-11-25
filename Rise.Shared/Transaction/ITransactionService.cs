using Rise.Shared.Cart;

namespace Rise.Shared.Transaction;

public interface ITransactionService
{
    Task AddTransactionScanOut(List<CartItem> cartItems);
    Task AddTransactionScanIn(List<CartItem> cartItems);
}