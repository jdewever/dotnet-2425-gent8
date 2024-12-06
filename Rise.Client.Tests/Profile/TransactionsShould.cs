using System.Collections.Generic;
using System.Threading.Tasks;
using Rise.Shared.Transaction;
using Xunit.Abstractions;

namespace Rise.Client.Profile;

public class TransactionsShould : TestContext
{
    public TransactionsShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        Services.AddSingleton<ITransactionService, FakeTransactionService>();
    }

    [Fact]
    public async Task RendersTableStructureCorrectly()
    {
        var transactions = await Services.GetService<ITransactionService>()!.GetRecentTransactions();

        var component = RenderComponent<Transactions>(parameters => parameters
            .Add(p => p.TransactionsList, transactions)
        );

        var caption = component.Find("caption");
        var headers = component.FindAll("th");

        Assert.Equal("Recente transacties", caption.TextContent.Trim());
        Assert.Equal("Producten", headers[0].TextContent.Trim());
        Assert.Equal("Datum", headers[1].TextContent.Trim());
        Assert.Equal("Type", headers[2].TextContent.Trim());
    }

    [Fact]
    public async Task RendersTransactionsCorrectly_WhenTransactionsListIsNotEmpty()
    {
        var transactions = await Services.GetService<ITransactionService>()!.GetRecentTransactions();

        var component = RenderComponent<Transactions>(parameters => parameters
            .Add(p => p.TransactionsList, transactions)
        );

        var rows = component.FindAll("tbody tr");

        Assert.Equal(transactions.Count, rows.Count);

        var columns = component.FindAll("td");
        var date = component.FindAll("td p");
        Assert.Equal(3, columns.Count);

        Assert.Equal("name x10", columns[0].TextContent.Trim());
        Assert.Equal(transactions[0].Date.ToShortDateString(), date[0].TextContent.Trim());
        Assert.Equal(transactions[0].Date.ToShortTimeString(), date[1].TextContent.Trim());
        Assert.Equal(transactions[0].Type, columns[2].TextContent.Trim());
    }

    [Fact]
    public void ShowsEmptyState_WhenTransactionsListIsEmpty()
    {
        var transactions = new List<TransactionDTO>();

        var component = RenderComponent<Transactions>(parameters => parameters
            .Add(p => p.TransactionsList, transactions)
        );

        var emptyRow = component.Find("tbody tr td");

        Assert.Equal("Geen transacties gevonden", emptyRow.TextContent.Trim());
        Assert.Equal("3", emptyRow.GetAttribute("colspan"));
    }
}