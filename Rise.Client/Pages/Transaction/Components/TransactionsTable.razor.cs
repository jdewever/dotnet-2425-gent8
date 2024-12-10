using Microsoft.AspNetCore.Components;
using Rise.Shared.Transaction;
using Rise.Shared.User;

namespace Rise.Client.Pages.Transaction.Components;

public partial class TransactionsTable : ComponentBase
{
    [Parameter] public IEnumerable<TransactionDTO> Transactions { get; set; } = null!;

    private static MarkupString GetProductNames(TransactionDTO transaction)
    {
        return new(string.Join("<br />", transaction.Products.Select(p => p.Product.Name)));
    }

    private static MarkupString GetProductAmounts(TransactionDTO transaction)
    {
        return new(string.Join("<br />", transaction.Products.Select(p => p.Quantity)));
    }
}