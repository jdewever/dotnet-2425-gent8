using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Rise.Client.Products;
using Rise.Shared.Products;

namespace Rise.Client.Pages.Admindashboard
{
    public class IndexShould : TestContext
    {
        private readonly IProductService FakeProductService = new FakeProductService();
        private readonly IRenderedComponent<InventoryView> Component;
        public IndexShould()
        {
            Services.AddSingleton(FakeProductService);
            Component = RenderComponent<InventoryView>();
        }

        [Fact]
        public void RendersCriticalSupplyCardCorrectly()
        {
            var LowStockCard = Component.Find("#low-stock-card");
            var amount = LowStockCard.Text().Split("\n")[0].Trim();
            var textfield = LowStockCard.Text().Split("\n")[1].Trim();

            Assert.Equal("1", amount);
            Assert.Equal("In kritieke voorraad", textfield);
        }

        [Fact]
        public void RendersTodaysBorrowCardCorrectly()
        {
            var LowStockCard = Component.Find("#reserved-products-card");
            var amount = LowStockCard.Text().Split("\n")[0].Trim();
            var textfield = LowStockCard.Text().Split("\n")[1].Trim();

            Assert.Equal("5", amount);
            Assert.Equal("Vandaag uitgeleend", textfield);
        }

        [Fact]
        public void RendersTodaysReturnCardCorrectly()
        {
            var LowStockCard = Component.Find("#returning-products-card");
            var amount = LowStockCard.Text().Split("\n")[0].Trim();
            var textfield = LowStockCard.Text().Split("\n")[1].Trim();

            Assert.Equal("3", amount);
            Assert.Equal("Vandaag terug te brengen", textfield);
        }

        [Fact]
        public void RendersTitleCorrectly()
        {
            var h1 = Component.Find("h1");
            var expected = "Producten met beperkte voorraad";

            Assert.Equal(expected, h1.TextContent.Trim());
        }

        [Fact]
        public void RendersTableHeaderCorrectly()
        {
            var headers = Component.FindAll("th");

            Assert.Equal("Barcode", headers[0].TextContent.Trim());
            Assert.Equal("Naam", headers[1].TextContent.Trim());
            Assert.Equal("Aantal in stock", headers[2].TextContent.Trim());
            Assert.Equal("Aantal in bestelling", headers[3].TextContent.Trim());
            Assert.Equal("Minimum gewenste voorraad", headers[4].TextContent.Trim());
        }

        [Fact]
        public async Task RendersTableContentCorrectly()
        {
            var dash = await FakeProductService.GetDashboardInfo();

            var rows = Component.FindAll("td");

            Assert.Single(dash.LowStockProducts.ToList()); // 1 product

            Assert.Equal("Barcode 8", rows[0].TextContent.Trim()); // Barcode
            Assert.Equal("Product 8", rows[1].TextContent.Trim()); // Naam
            Assert.Equal("8", rows[2].TextContent.Trim()); // Aantal in stock
            Assert.Equal("8", rows[3].TextContent.Trim()); // Aantal in bestelling
            Assert.Equal("32", rows[4].TextContent.Trim()); // Minimum gewenste voorraad
        }
    }
}