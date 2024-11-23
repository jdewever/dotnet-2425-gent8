using System.Transactions;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Transaction;

namespace Rise.Client.Profile;

public partial class Transactions : ComponentBase
{
    [Parameter, EditorRequired] public required IEnumerable<TransactionDto> TransactionsList { get; set; }

    private string getSummeryProducts(TransactionDto transaction)
    {
        var summeryProducts = string.Empty;
        foreach (var (product, amount) in transaction.Products.Take(2))
        {
            summeryProducts += $"{product} x {amount}\n";
        }

        if (transaction.Products.Count > 2)
        {
            summeryProducts += $"+{transaction.Products.Count - 2} producten";
        }
        return summeryProducts;
    }
}