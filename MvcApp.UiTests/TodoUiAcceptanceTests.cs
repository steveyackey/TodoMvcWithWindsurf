using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace MvcApp.UiTests
{
    public class TodoUiAcceptanceTests : IAsyncLifetime
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IBrowserContext _context;
        private IPage _page;
        private const string AppUrl = "http://localhost:5005/Todos";

        public async Task InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
            await _browser.DisposeAsync();
            _playwright.Dispose();
        }

        [Fact]
        public async Task CanAddTodoWithAllFields()
        {
            // Wait until the app is reachable (max 30s)
            bool reachable = false;
            for (int i = 0; i < 30; i++)
            {
                try
                {
                    var response = await _page.GotoAsync(AppUrl, new() { Timeout = 1000 });
                    if (response != null && response.Ok && (await _page.TitleAsync()).Contains("Todo")) { reachable = true; break; }
                }
                catch (System.Exception) { await Task.Delay(1000); }
            }
            Assert.True(reachable, $"App not reachable at {AppUrl}");

            // Check for known static text
            var pageTitle = await _page.TitleAsync();
            Assert.Contains("Todo", pageTitle);
            var bodyText = await _page.InnerTextAsync("body");
            Assert.Contains("Todo List", bodyText);

            await _page.WaitForSelectorAsync("input[name=title]");
            await _page.FillAsync("input[name=title]", "UI Acceptance Test Todo");
            var dueDate = System.DateTime.Today.AddDays(2).ToString("yyyy-MM-dd");
            await _page.FillAsync("input[name=dueDate]", dueDate);
            await _page.SelectOptionAsync("select[name=priority]", new[] { "High" });
            await _page.FillAsync("input[name=category]", "Playwright");
            await _page.ClickAsync("button[type=submit]");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            bodyText = await _page.InnerTextAsync("body");
            if (!bodyText.Contains("UI Acceptance Test Todo"))
            {
                // Always print body text to test output for debugging
                System.Console.WriteLine("PAGE BODY:\n" + bodyText);
                Assert.True(false, $"Todo was not added. Page body: {bodyText}");
            }
            var todoItem = await _page.QuerySelectorAsync("li.list-group-item:has-text('UI Acceptance Test Todo')");
            Assert.NotNull(todoItem);
            var htmlContent = await todoItem.InnerHTMLAsync();
            Assert.Contains("High", htmlContent);
            Assert.Contains("Playwright", htmlContent);
            Assert.Contains(System.DateTime.Today.AddDays(2).ToShortDateString(), htmlContent);
        }
    }
}
