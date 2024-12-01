using Microsoft.AspNetCore.Components;
using Rise.Shared.Transaction;

namespace Rise.Client.Pages.Transaction.Components;

public partial class TransactionsTable : ComponentBase
{
    [Parameter] public IEnumerable<TransactionDTO>? Transactions { get; set; }

    private string GetProductNames(TransactionDTO transaction)
    {
        return string.Join(", ", transaction.Products.Select(p => p.Product.Name));
    }
}