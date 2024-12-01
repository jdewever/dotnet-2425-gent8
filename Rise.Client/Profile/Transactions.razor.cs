using Microsoft.AspNetCore.Components;
using Rise.Shared.Transaction;

namespace Rise.Client.Profile;

public partial class Transactions : ComponentBase
{
    [Parameter, EditorRequired] public required IEnumerable<TransactionDTO> TransactionsList { get; set; }

    private string GetSummeryProducts(TransactionDTO transaction)
    {
        var summeryProducts = string.Empty;
        if (transaction.Products.Count() <= 2)
        {
            return transaction.Products.Aggregate(summeryProducts,
                (current, transactionProduct) =>
                    current + $"{transactionProduct.Product.Name} x{transactionProduct.Quantity}\n");
        }

        summeryProducts = transaction.Products.Take(2).Aggregate(summeryProducts,
            (current, transactionProduct) =>
                current + $"{transactionProduct.Product.Name} x{transactionProduct.Quantity}\n");

        summeryProducts += $"+{transaction.Products.Count() - 2} producten";

        return summeryProducts;
    }
}