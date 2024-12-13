using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests
{
    public class DashboardTest : PageTest
    {
        private IPage page = null!;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await PageManager.Instance.InitializeAsync();
            page = PageManager.Instance.GetAdminPage();
        }

        [SetUp]
        public async Task SetUp()
        {
            // Navigeer naar de juiste pagina zonder nieuwe page aan te maken
            await PageManager.Instance.NavigateAsync(page, "https://localhost:5001/");
        }

        [Test]
        public async Task DashboardCartItemsRenderCorrectly()
        {
            await Expect(page.Locator("#low-stock-card")).ToBeVisibleAsync();
            await Expect(page.GetByText("In kritieke voorraad")).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Img, new() { Name = "In kritieke voorraad icon" })).ToBeVisibleAsync();
            await Expect(page.Locator("#reserved-products-card")).ToBeVisibleAsync();
            await Expect(page.GetByText("Vandaag uitgeleend")).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Img, new() { Name = "Vandaag uitgeleend icon" })).ToBeVisibleAsync();
            await Expect(page.Locator("#returning-products-card")).ToBeVisibleAsync();
            await Expect(page.GetByText("Vandaag terug te brengen")).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Img, new() { Name = "Vandaag terug te brengen icon" })).ToBeVisibleAsync();
        }

        [Test]
        public async Task DashboardTitleRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Producten met beperkte voorraad" })).ToBeVisibleAsync();
        }

        [Test]
        public async Task DashboardTableHeaderRendersCorrectly()
        {
            await page.Locator("Table").IsVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Barcode" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Naam" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Aantal in stock" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Aantal in bestelling" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Minimum gewenste voorraad" })).ToBeVisibleAsync();
        }
    }
}