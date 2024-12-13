using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Rise.Persistence;
using DotNetEnv;

namespace PlaywrightTests
{
    public class CategoriesTest : PageTest
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

            // Navigate to the correct page without creating a new page
            await PageManager.Instance.NavigateAsync(page, "https://localhost:5001/manage/categories");
        }

        [Test]
        public async Task ManageCategoriesNavigatorRendersCorrectly()
        {
            await page.GetByRole(AriaRole.Button, new() { Name = "Categorieën" }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Gebruikers" }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Verborgen Producten" }).IsVisibleAsync();
        }

        [Test]
        public async Task ManageCategoriesTitleRendersCorrectly()
        {
            await page.GetByText("Categorieën").IsVisibleAsync();
        }

        [Test]
        public async Task ManageCategoriesAddingButtonRendersCorrectly()
        {
            await page.GetByRole(AriaRole.Button, new() { Name = "Toevoegen" }).IsVisibleAsync();
        }

        [Test]
        public async Task ManageCategoriesTableHeaderRendersCorrectly()
        {
            await page.GetByRole(AriaRole.Cell, new() { Name = "Naam" }).IsVisibleAsync();
            await page.GetByRole(AriaRole.Cell, new() { Name = "Acties" }).IsVisibleAsync();
        }

        [Test]
        public async Task ManageCategoriesTableContextRendersCorrectly()
        {
            await page.Locator("Table").IsVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Diagnostische apparatuur" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Diagnostische apparatuur" }).GetByRole(AriaRole.Button).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Diagnostische apparatuur" }).GetByRole(AriaRole.Button).Nth(1)).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Injectie en toediening" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Injectie en toediening" }).GetByRole(AriaRole.Button).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Injectie en toediening" }).GetByRole(AriaRole.Button).Nth(1)).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Wondverzorging" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Wondverzorging" }).GetByRole(AriaRole.Button).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Wondverzorging" }).GetByRole(AriaRole.Button).Nth(1)).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Hygiëne" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Hygiëne" }).GetByRole(AriaRole.Button).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Hygiëne" }).GetByRole(AriaRole.Button).Nth(1)).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Mobiliteitshulpmiddelen" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Mobiliteitshulpmiddelen" }).GetByRole(AriaRole.Button).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Mobiliteitshulpmiddelen" }).GetByRole(AriaRole.Button).Nth(1)).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Chirurgische instrumenten" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Chirurgische instrumenten" }).GetByRole(AriaRole.Button).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Chirurgische instrumenten" }).GetByRole(AriaRole.Button).Nth(1)).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Fysiotherapieapparatuur" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Fysiotherapieapparatuur" }).GetByRole(AriaRole.Button).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Fysiotherapieapparatuur" }).GetByRole(AriaRole.Button).Nth(1)).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Ademhalingshulpmiddelen" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Ademhalingshulpmiddelen" }).GetByRole(AriaRole.Button).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Ademhalingshulpmiddelen" }).GetByRole(AriaRole.Button).Nth(1)).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Medicatie en medicijnkast" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Medicatie en medicijnkast" }).GetByRole(AriaRole.Button).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "Medicatie en medicijnkast" }).GetByRole(AriaRole.Button).Nth(1)).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "EHBO-kits" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "EHBO-kits" }).GetByRole(AriaRole.Button).First).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { Name = "EHBO-kits" }).GetByRole(AriaRole.Button).Nth(1)).ToBeVisibleAsync();
        }
    }
}