using Microsoft.Playwright;
using DotNetEnv;

namespace PlaywrightTests
{
    [SetUpFixture]
    public class SaveAuthState
    {
        [OneTimeSetUp]
        public async Task SaveAuthenticationStates()
        {
            Env.Load("../../../../.env");

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });

            await SaveAdminAuthenticationState(browser);
            await SaveInventoryManagerAuthenticationState(browser);
            await SaveUserAuthenticationState(browser);
        }

        private async Task SaveAdminAuthenticationState(IBrowser browser)
        {
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://localhost:5001");
            await page.GetByPlaceholder("Geef uw email in").FillAsync(Environment.GetEnvironmentVariable("ADMIN_EMAIL")!);
            await page.GetByPlaceholder("Geef uw wachtwoord in").FillAsync(Environment.GetEnvironmentVariable("ADMIN_PASSWORD")!);
            await page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
            await page.WaitForSelectorAsync("text=Log Out");
            await context.StorageStateAsync(new BrowserContextStorageStateOptions
            {
                Path = "admin-auth-state.json"
            });
        }

        private async Task SaveInventoryManagerAuthenticationState(IBrowser browser)
        {
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://localhost:5001");
            await page.GetByPlaceholder("Geef uw email in").FillAsync(Environment.GetEnvironmentVariable("INVENTORY_MANAGER_EMAIL")!);
            await page.GetByPlaceholder("Geef uw wachtwoord in").FillAsync(Environment.GetEnvironmentVariable("INVENTORY_MANAGER_PASSWORD")!);
            await page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
            await page.WaitForSelectorAsync("text=Log Out");
            await context.StorageStateAsync(new BrowserContextStorageStateOptions
            {
                Path = "inventorymanager-auth-state.json"
            });
        }

        private async Task SaveUserAuthenticationState(IBrowser browser)
        {
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://localhost:5001");
            await page.GetByPlaceholder("Geef uw email in").FillAsync(Environment.GetEnvironmentVariable("USER_EMAIL")!);
            await page.GetByPlaceholder("Geef uw wachtwoord in").FillAsync(Environment.GetEnvironmentVariable("USER_PASSWORD")!);
            await page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
            await page.WaitForSelectorAsync("text=Log Out");
            await context.StorageStateAsync(new BrowserContextStorageStateOptions
            {
                Path = "user-auth-state.json"
            });
        }
    }
}