using System.Threading.Tasks;
using Rise.Shared.Transaction;

namespace Rise.Client.Pages.Transaction
{
    public class IndexShould : TestContext
    {
        private readonly ITransactionService FakeTransactionService = new FakeTransactionService();
        private readonly IRenderedComponent<Index> Component;

        public IndexShould()
        {
            Services.AddSingleton(FakeTransactionService);
            Component = RenderComponent<Index>();
        }

        [Fact]
        public void RendersTitleCorrectly()
        {
            var h1 = Component.Find("h1");

            Assert.Equal("Transacties", h1.TextContent.Trim());
        }

        [Fact]
        public void RendersTableHeaderCorrectly()
        {
            var headers = Component.FindAll("th");

            Assert.Equal("Producten", headers[0].TextContent.Trim());
            Assert.Equal("Hoeveelheden", headers[1].TextContent.Trim());
            Assert.Equal("Datum", headers[2].TextContent.Trim());
            Assert.Equal("Type", headers[3].TextContent.Trim());
        }

        [Fact]
        public async Task RendersTableContentCorrectly()
        {
            var transactions = await FakeTransactionService.GetRecentTransactions();

            var rows = Component.FindAll("td");

            Assert.Equal(3, transactions.Count);

            Assert.Equal("productTestName", rows[0].TextContent.Trim());
            Assert.Equal("4", rows[1].TextContent.Trim());
            Assert.Equal(Common.GetRoundedDate(-1, 2).ToString(), rows[2].TextContent.Trim());
            Assert.Equal("ScanOut", rows[3].TextContent.Trim());

            Assert.Equal("productTestNameproductTestName2", rows[4].TextContent.Trim());
            Assert.Equal("410", rows[5].TextContent.Trim());
            Assert.Equal(Common.GetRoundedDate(-4, 2).ToString(), rows[6].TextContent.Trim());
            Assert.Equal("ScanOut", rows[7].TextContent.Trim());

            Assert.Equal("productTestNameproductTestName2", rows[8].TextContent.Trim());
            Assert.Equal("219", rows[9].TextContent.Trim());
            Assert.Equal(Common.GetRoundedDate(15, 2).ToString(), rows[10].TextContent.Trim());
            Assert.Equal("ScanIn", rows[11].TextContent.Trim());
        }

    }
}