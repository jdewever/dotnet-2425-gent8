using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests
{
    public class ReservationsStudentTest : PageTest
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
            await PageManager.Instance.NavigateAsync(page, "https://localhost:5001/reservations");
        }

        [Test]
        public async Task ReservationsHeaderRendersCorrectly()
        {
            await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Reserveringen" })).ToBeVisibleAsync();
        }

        [Test]
        public async Task UserReservationsTableHeaderRendersCorrectly()
        {
            await page.Locator("ReservationsTable").IsVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Naam" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Startdatum" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Einddatum" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "Acties" })).ToBeVisibleAsync();
        }
    }
}