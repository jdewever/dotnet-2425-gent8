using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests
{
    public class InvManagerLayoutTest : PageTest
    {
        private IPage page = null!;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await PageManager.Instance.InitializeAsync();
            page = PageManager.Instance.GetManagerPage();
        }

        [SetUp]
        public async Task SetUp()
        {
            // Navigeer naar de juiste pagina zonder nieuwe page aan te maken
            await PageManager.Instance.NavigateAsync(page, "https://localhost:5001/");
        }

        [Test]
        public async Task InvManagerLayoutSidebarIconRendersCorrectly()
        {
            await page.GetByRole(AriaRole.Img, new() { Name = "Rise" }).IsVisibleAsync();
        }

        [Test]
        public async Task InvManagerLayoutSidebarPagesRendersCorrectly()
        {
            await page.GetByRole(AriaRole.Link, new() { Name = "Voorraadbeheer Voorraadbeheer" }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Scannen", Exact = true }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Reserveren" }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Scannen Scannen" }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Reserveringen Reserveringen" }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Transacties Transacties" }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Beheren Beheren" }).IsVisibleAsync();
        }

        [Test]
        public async Task InvManagerLayoutSidebarLogOutRendersCorrectly()
        {
            await page.GetByRole(AriaRole.Link, new() { Name = "Log Out" }).IsVisibleAsync();
        }

        [Test]
        public async Task InvManagerLayoutProfileHeaderRendersCorrectly()
        {
            await page.GetByRole(AriaRole.Img, new() { Name = "Profile Picture" }).First.IsVisibleAsync();
            await page.GetByText("Inventory Manager").First.IsVisibleAsync();
        }
    }
}