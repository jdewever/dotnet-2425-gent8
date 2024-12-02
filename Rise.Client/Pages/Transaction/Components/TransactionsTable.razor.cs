using Microsoft.AspNetCore.Components;
using Rise.Shared.Transaction;

namespace Rise.Client.Pages.Transaction.Components;

public partial class TransactionsTable : ComponentBase
{
    [Parameter] public IEnumerable<TransactionDTO>? Transactions { get; set; }

    private static string GetProductNames(TransactionDTO transaction)
    {
        var productNames = transaction.Products.Select(p => p.Product.Name).ToList();

        return string.Join(", ", transaction.Products.Select(p => p.Product.Name));
    }

    private static string GetProductAmounts(TransactionDTO transaction)
    {
        return string.Join(", ", transaction.Products.Select(p => p.Quantity));
    }
}