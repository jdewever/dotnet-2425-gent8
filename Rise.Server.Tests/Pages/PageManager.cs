using Microsoft.Playwright;

namespace PlaywrightTests
{
    public class PageManager
    {
        private static PageManager _instance = null!;
        private static readonly object _lock = new object();

        private IPlaywright _playwright;
        private IBrowser _browser;
        private IBrowserContext _adminContext;
        private IBrowserContext _userContext;
        private IBrowserContext _managerContext;

        // Cache de pages
        private IPage _adminPage;
        private IPage _userPage;
        private IPage _managerPage;

        private bool _initialized;

        private PageManager() { }

        public static PageManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new PageManager();
                    }
                }
                return _instance;
            }
        }

        public async Task InitializeAsync()
        {
            if (_initialized) return;

            lock (_lock)
            {
                if (_initialized) return;
                _initialized = true;
            }

            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            // Maak contexts aan
            _adminContext = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                StorageStatePath = "admin-auth-state.json"
            });

            _userContext = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                StorageStatePath = "user-auth-state.json"
            });

            _managerContext = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                StorageStatePath = "inventorymanager-auth-state.json"
            });

            // Maak één page per context aan
            _adminPage = await _adminContext.NewPageAsync();
            _userPage = await _userContext.NewPageAsync();
            _managerPage = await _managerContext.NewPageAsync();
        }

        // Getters voor de pages die de cached instances returnen
        public IPage GetAdminPage()
        {
            if (!_initialized) throw new InvalidOperationException("PageManager not initialized");
            return _adminPage;
        }

        public IPage GetUserPage()
        {
            if (!_initialized) throw new InvalidOperationException("PageManager not initialized");
            return _userPage;
        }

        public IPage GetManagerPage()
        {
            if (!_initialized) throw new InvalidOperationException("PageManager not initialized");
            return _managerPage;
        }

        // Optioneel: methode om naar een nieuwe URL te navigeren
        public async Task NavigateAsync(IPage page, string url)
        {
            await page.GotoAsync(url);
        }
    }
}