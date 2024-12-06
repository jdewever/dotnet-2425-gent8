using Microsoft.AspNetCore.Components;
using Rise.Shared.Transaction;

namespace Rise.Client.Pages.Transaction.Components;

public partial class TransactionsTable : ComponentBase
{
    [Parameter] public IEnumerable<TransactionDTO>? Transactions { get; set; }

    private static MarkupString GetProductNames(TransactionDTO transaction)
    {
        return new(string.Join("<br />", transaction.Products.Select(p => p.Product.Name)));
    }

    private static MarkupString GetProductAmounts(TransactionDTO transaction)
    {
        return new(string.Join("<br />", transaction.Products.Select(p => p.Quantity)));
    }
}