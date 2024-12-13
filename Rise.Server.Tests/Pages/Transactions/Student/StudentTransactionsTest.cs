using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests
{
    public class StudentTransactionsTest : PageTest
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
            await PageManager.Instance.NavigateAsync(page, "https://localhost:5001/transactions");
        }

        [Test]
        public async Task TransactionsHeaderRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Transacties" })).ToBeVisibleAsync();
        }

        [Test]
        public async Task TransactionsTableHeaderRendersCorrectly()
        {
            await page.Locator("TransactionsTable").IsVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Producten" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Hoeveelheden" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Datum" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Type" })).ToBeVisibleAsync();
        }
    }
}