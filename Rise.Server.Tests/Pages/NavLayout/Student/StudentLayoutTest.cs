using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests
{
    public class StudentLayoutTest : PageTest
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
            await PageManager.Instance.NavigateAsync(page, "https://localhost:5001/");
        }

        [Test]
        public async Task StudentLayoutSidebarIconRendersCorrectly()
        {
            await page.GetByRole(AriaRole.Img, new() { Name = "Rise" }).IsVisibleAsync();
        }

        [Test]
        public async Task StudentLayoutSidebarPagesRendersCorrectly()
        {
            await page.GetByRole(AriaRole.Link, new() { Name = "Voorraadbeheer Voorraadbeheer" }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Scannen", Exact = true }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Reserveren" }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Scannen Scannen" }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Reserveringen Reserveringen" }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Transacties Transacties" }).IsVisibleAsync();
        }

        [Test]
        public async Task StudentLayoutSidebarLogOutRendersCorrectly()
        {
            await page.GetByRole(AriaRole.Link, new() { Name = "Log Out" }).IsVisibleAsync();
        }

        [Test]
        public async Task StudentLayoutProfileHeaderRendersCorrectly()
        {
            await page.GetByRole(AriaRole.Img, new() { Name = "Profile Picture" }).First.IsVisibleAsync();
            await page.GetByText("Test Student").First.IsVisibleAsync();
        }
    }
}