using Microsoft.AspNetCore.Components;
using Rise.Shared.Transaction;

namespace Rise.Client.Pages.Transaction.Components;

public partial class TransactionsTable : ComponentBase
{
    [Parameter] public IEnumerable<TransactionDTO>? Transactions { get; set; }

    private static string GetProductNames(TransactionDTO transaction)
    {
        return string.Join("\n", transaction.Products.Select(p => p.Product.Name));
    }

    private static string GetProductAmounts(TransactionDTO transaction)
    {
        return string.Join("\n", transaction.Products.Select(p => p.Quantity));
    }
}