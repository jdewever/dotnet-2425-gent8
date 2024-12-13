using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests
{
    public class AdminProfileLayoutTest : PageTest
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
            await PageManager.Instance.NavigateAsync(page, "https://localhost:5001/profile");
        }

        [Test]
        public async Task AdminProfileRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Article).GetByRole(AriaRole.Img, new() { Name = "Profile Picture" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Article).GetByText("Admin", new() { Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByText("admin@hogent.be")).ToBeVisibleAsync();

        }

    }
}