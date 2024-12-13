using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests
{
    public class UserProductsLayoutTest : PageTest
    {
        private IPage page = null!;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await PageManager.Instance.InitializeAsync();
            page = PageManager.Instance.GetUserPage();
        }

        [SetUp]
        public async Task SetUp()
        {
            // Navigeer naar de juiste pagina zonder nieuwe page aan te maken
            await PageManager.Instance.NavigateAsync(page, "https://localhost:5001/products");
        }

        [Test]
        public async Task UserProductsFilterRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Article).Locator("svg").First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Article).Locator("svg").Nth(1)).ToBeVisibleAsync();
            await Expect(page.Locator("label div")).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Article).Locator("svg").Nth(2)).ToBeVisibleAsync();
            await Expect(page.GetByPlaceholder("zoektermen")).ToBeVisibleAsync();
        }

        [Test]
        public async Task UserProductsPaginationRendersCorrectly()
        {
            await Expect(page.GetByText("Pagina 1 van")).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Combobox)).ToBeVisibleAsync();
            await Expect(page.Locator("div").Filter(new() { HasText = "5 per pagina10 per pagina14" }).Nth(3)).ToBeVisibleAsync();
            await Expect(page.GetByText("Pagina 1 van 1 5 per pagina10")).ToBeVisibleAsync();
        }

        [Test]
        public async Task UserProductsItemViewRendersCorrectly()
        {
            await Expect(page.GetByText("Pagina 1 van")).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Combobox)).ToBeVisibleAsync();
            await Expect(page.Locator("div").Filter(new() { HasText = "5 per pagina10 per pagina14" }).Nth(3)).ToBeVisibleAsync();
            await Expect(page.GetByText("Pagina 1 van 1 5 per pagina10")).ToBeVisibleAsync();
        }

        [Test]
        public async Task UserProductsTableViewRendersCorrectly()
        {
            await page.GetByRole(AriaRole.Article).Locator("svg").Nth(1).ClickAsync();
            //await page.Locator("Table").IsVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Naam" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Beschrijving" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "In Stock" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Lokaal" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Acties" })).ToBeVisibleAsync();
        }

        [Test]
        public async Task UserProductDetailModalShouldShowCorrectly() 
        {
            await page.Locator(".h-32").First.ClickAsync();
            await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Injectienaald" })).ToBeVisibleAsync();
            await Expect(page.GetByText("In stock:")).ToBeVisibleAsync();
            await Expect(page.GetByText("Op bestelling:")).ToBeVisibleAsync();
            await Expect(page.GetByText("Lokaal:")).ToBeVisibleAsync();
            await Expect(page.GetByText("Een injectienaald is een")).ToBeVisibleAsync();
            await Expect(page.GetByText("Injectie en toediening")).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Wijzigen" })).Not.ToBeVisibleAsync();
            await page.Locator(".absolute > .bg-gray-200").ClickAsync();
        }
    }
}