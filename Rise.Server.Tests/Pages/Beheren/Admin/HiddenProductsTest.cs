using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests
{
    public class HiddenProductsTest : PageTest
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
            await PageManager.Instance.NavigateAsync(page, "https://localhost:5001/manage/hiddenproducts");
        }

        [Test]
        public async Task ManageHiddenProductsNavigatorRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Categorieën" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Gebruikers" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Verborgen Producten" })).ToBeVisibleAsync();
        }

        [Test]
        public async Task ManageHiddenProductsAddingButtonRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Toevoegen" })).ToBeVisibleAsync();
        }

        [Test]
        public async Task ManageHiddenProductsTableHeaderRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Barcode" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Naam" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Beschrijving" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "In Stock" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "In Bestelling" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Lokaal" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Acties" })).ToBeVisibleAsync();
        }

        [Test]
        public async Task ManageHiddenProductsTableContextRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "1463097558004" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Elektrisch ziekenhuisbed", Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Een elektrisch ziekenhuisbed" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "8", Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "4", Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "A.401" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "1463097558004 Elektrisch" }).GetByRole(AriaRole.Button).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Tonen" }).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "1511125934709" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Nietjespistool", Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Een nietjespistool is een" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "3", Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "0", Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "B.4012" }).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "1511125934709 Nietjespistool" }).GetByRole(AriaRole.Button).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Tonen" }).Nth(1)).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "5034056696899" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Bloeddrukmeter voor pols" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Een polsbloeddrukmeter is een" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "20" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "50", Exact = true })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "B.4012" }).Nth(1)).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "5034056696899 Bloeddrukmeter" }).GetByRole(AriaRole.Button).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Tonen" }).Nth(2)).ToBeVisibleAsync();
        }

    }
}