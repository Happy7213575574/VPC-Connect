using System;
using System.Threading;
using System.Threading.Tasks;
using ConnectApp.Communication;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PortalApi.Tests.Helpers
{
    public class PortalInteractionHelper : IAsyncDisposable
    {
        public IPlaywright playwright;
        public IBrowser browser;
        public IPage page;

        public async Task InitAsync(bool headless = true)
        {
            playwright = playwright ?? await Playwright.CreateAsync();
            var opts = new BrowserTypeLaunchOptions()
            {
                Headless = headless
            };
            browser = browser ?? await playwright.Firefox.LaunchAsync(opts);
            page = await NewPageAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncInternal();
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncInternal()
        {
            await page.CloseAsync();
            await browser.CloseAsync();
        }

        public async Task<IPage> NewPageAsync() => await browser.NewPageAsync();

        public async Task<IResponse> VisitAsync(string uri, bool requireOk = true, string expectedUri = null)
            => await VisitAsync(new Uri(uri), requireOk, expectedUri != null ? new Uri(expectedUri) : null);

        public async Task<IResponse> VisitAsync(Uri uri, bool confirmOk = true, Uri expectedUri = null)
        {
            Console.WriteLine($"Opening page: {uri.ToString()}");
            var response = await page.GotoAsync(uri.ToString());

            //if (expectedUri != null) { await page.WaitForURLAsync(expectedUri.ToString()); }

            var foundUri = new Uri(page.Url);
            if (confirmOk)
            {
                Assert.IsTrue(response.Ok, $"response not OK: {response.Status} {response.StatusText}");
            }

            if (expectedUri != null)
            {
                var result = Uri.Compare(expectedUri, foundUri,
                  UriComponents.Scheme | UriComponents.Host | UriComponents.Path,
                  UriFormat.SafeUnescaped,
                  StringComparison.OrdinalIgnoreCase);
                Assert.AreEqual(0, result, $"Expected: {expectedUri}, Found: {page.Url}");
            }

            return response;
        }

        public async Task<IResponse> VisitLoginAsync()
        {
            return await VisitAsync(PortalUris.PortalWeb_LoginUri);
        } 

        public async Task ConfirmTextAsync(string text, int count = 1) => await ConfirmElementAsync($":has-text(\"{text}\")", count);

        public async Task ConfirmElementTextAsync(string element, string text, int count = 1) =>
            await ConfirmElementAsync($"{element}:has-text(\"{text}\")", count);

        public async Task ConfirmElementAsync(string selector, int count = 1) =>
            Assert.AreEqual(await page.Locator(selector).CountAsync(), count, $"Not found: {selector}");

        public ILocator FindText(string text) => page.Locator($":has-text(\"{text}\")");

        public async Task LogInAsync()
        {
            await VisitLoginAsync();
            await ConfirmElementTextAsync("h1", "Marshall Volunteer Portal Sign On");

            await page.Locator("id=UserName").FillAsync(AccountHelper.Username);
            await page.Locator("id=Password").FillAsync(AccountHelper.Password);
            await page.Locator("button[type=\"submit\"]").ClickAsync();

            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await page.WaitForURLAsync(AccountHelper.SignedInUrl);

            //Console.WriteLine(page.ContentAsync());

            await ConfirmElementTextAsync("h3", AccountHelper.FullName);
            await ConfirmElementTextAsync("strong","My Police force");
            
            await ConfirmElementTextAsync("h2", "My Latest Notifications");
            await ConfirmElementTextAsync("h2", "My Conversations");
            await ConfirmElementTextAsync("h2", "My Booked Events");
            await ConfirmElementTextAsync("h2", "Upcoming Events");
            await ConfirmElementTextAsync("p", "You are logged in as");
        }

        public async Task VisitDiaryAsync()
        {
            await VisitAsync(PortalUris.PortalWeb_DiaryUri, true, PortalUris.PortalWeb_DiaryUri);
            await ConfirmElementTextAsync("h2", "Events Diary");
        }

        public async Task VisitCalendarAsync()
        {
            await VisitAsync(PortalUris.PortalWeb_CalendarUri, true, PortalUris.PortalWeb_CalendarUri);
            await ConfirmElementTextAsync("h2", "Event Calendar");
        }

        public async Task VisitConversationsAsync()
        {
            await VisitAsync(PortalUris.PortalWeb_ConversationsUri, true, PortalUris.PortalWeb_ConversationsUri);
            await ConfirmElementTextAsync("h2", "My Conversations");
        }

        public async Task VisitMessageBoardsAsync()
        {
            await VisitAsync(PortalUris.PortalWeb_MessageBoardsUri, true, PortalUris.PortalWeb_MessageBoardsUri);
            await ConfirmElementTextAsync("h2", "Message Boards");
        }

        public async Task VisitResourcesAsync()
        {
            await VisitAsync(PortalUris.PortalWeb_ResourcesUri, true, PortalUris.PortalWeb_ResourcesUri);
            await ConfirmElementTextAsync("h2", "Resource Library");
        }


    }
}
