using Rise.Shared.Cart;

namespace Rise.Shared.Transaction;
public interface ITransactionItemService
{
    Task AddTransactionItems(int transactionId, List<CartItem> transactionItems);
}

