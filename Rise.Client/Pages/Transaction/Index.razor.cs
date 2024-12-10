using Microsoft.AspNetCore.Components;
using Rise.Shared.Transaction;

namespace Rise.Client.Pages.Transaction;

public partial class Index : ComponentBase
{
    [Inject] private ITransactionService TransactionService { get; set; } = null!;
    private IEnumerable<TransactionDTO>? Transactions { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Transactions = await TransactionService.GetRecentTransactions();
    }
}