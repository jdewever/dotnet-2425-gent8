using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests
{
    public class AdminProductsReserverenLayoutTest : PageTest
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
            await PageManager.Instance.NavigateAsync(page, "https://localhost:5001/reserve");
        }

        [Test]
        public async Task AdminProductsFilterRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Article).Locator("svg").First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Article).Locator("svg").Nth(1)).ToBeVisibleAsync();
            await Expect(page.Locator("label div")).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Article).Locator("svg").Nth(2)).ToBeVisibleAsync();
            await Expect(page.GetByPlaceholder("zoektermen")).ToBeVisibleAsync();
        }

        [Test]
        public async Task AdminProductsAddProductRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "+" })).ToBeVisibleAsync();
        }

        [Test]
        public async Task AdminProductsPaginationRendersCorrectly()
        {
            await Expect(page.GetByText("Pagina 1 van")).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Combobox)).ToBeVisibleAsync();
            await Expect(page.Locator("div").Filter(new() { HasText = "5 per pagina10 per pagina14" }).Nth(3)).ToBeVisibleAsync();
            await Expect(page.GetByText("Pagina 1 van 1 5 per pagina10")).ToBeVisibleAsync();
        }

        [Test]
        public async Task AdminProductsItemViewRendersCorrectly()
        {
            await Expect(page.GetByText("Bloeddrukmeter")).ToBeVisibleAsync();
            await Expect(page.Locator(".h-32").First).ToBeVisibleAsync();
            await Expect(page.GetByText("20", new() { Exact = true }).First).ToBeVisibleAsync();
            await Expect(page.Locator(".product > .bg-gray-200").First).ToBeVisibleAsync();
        }

        [Test]
        public async Task AdminProductsTableViewRendersCorrectly()
        {
            await page.GetByRole(AriaRole.Article).Locator("svg").Nth(1).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Code" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Naam" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Beschrijving" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "In Stock" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "In Bestelling" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Lokaal" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Acties" })).ToBeVisibleAsync();
        }

        [Test]
        public async Task AdminProductDetailModalShouldShowCorrectly() 
        {
            await page.Locator(".h-32").First.ClickAsync();
            await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Bloeddrukmeter" })).ToBeVisibleAsync();
            await Expect(page.GetByText("In stock:")).ToBeVisibleAsync();
            await Expect(page.GetByText("Op bestelling:")).ToBeVisibleAsync();
            await Expect(page.GetByText("Lokaal:")).ToBeVisibleAsync();
            await Expect(page.GetByText("Een apparaat om de")).ToBeVisibleAsync();
            await Expect(page.GetByText("Diagnostische apparatuur")).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Wijzigen" })).ToBeVisibleAsync();
            await Expect(page.Locator(".absolute > button:nth-child(2)")).ToBeVisibleAsync();
        }
    }
}