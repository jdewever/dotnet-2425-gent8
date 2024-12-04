using Microsoft.AspNetCore.Components;
using Rise.Shared.Transaction;

namespace Rise.Client.Pages.Transaction.Components;

public partial class TransactionsTable : ComponentBase
{
    [Parameter] public IEnumerable<TransactionDTO>? Transactions { get; set; }

    private static MarkupString GetProductNames(TransactionDTO transaction)
    {
        var productNames = transaction.Products.Select(p => p.Product.Name).ToList();
        MarkupString result = new(string.Join("<br />", productNames));

        return result;
    }

    private static MarkupString GetProductAmounts(TransactionDTO transaction)
    {
        return new(string.Join("<br />", transaction.Products.Select(p => p.Quantity)));
    }
}