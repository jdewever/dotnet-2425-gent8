using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests
{
    public class ScanningLayoutTest : PageTest
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
            await PageManager.Instance.NavigateAsync(page, "https://localhost:5001/scan");
        }

        [Test]
        public async Task ScanningPageProductViewRendersCorrectly()
        {
            await Expect(page.GetByText("In stock")).ToBeVisibleAsync();
            await Expect(page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Product ingeven \\.\\.\\.$") })).ToBeVisibleAsync();
        }

        [Test]
        public async Task ScanningPageBarcodeSearchRendersCorrectly()
        {
            await Expect(page.GetByText("Barcode")).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Article).Locator("path")).ToBeVisibleAsync();
            await Expect(page.GetByPlaceholder("123456789")).ToBeVisibleAsync();
        }

        [Test]
        public async Task ScanningPageAmountsRenderCorrectly()
        {
            await Expect(page.GetByText("0", new() { Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByText("-100 -10 -1 +1 +10 +")).ToBeVisibleAsync();
        }

        [Test]
        public async Task ScanningPageAddingButtonRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Voeg toe" })).ToBeVisibleAsync();
        }

        [Test]
        public async Task ScanningPageCartSliderRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Scan uit" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Scan in" })).ToBeVisibleAsync();
        }

        [Test]
        public async Task ScanningPageCartTableHeaderRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Naam" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Hoeveelheid" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Verwijderen" })).ToBeVisibleAsync();
        }

        [Test]
        public async Task ScanningPageCheckoutButtonRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Bevestig" })).ToBeVisibleAsync();
        }
    }
}