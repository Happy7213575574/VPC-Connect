using System;
using System.Threading.Tasks;
using ConnectApp.Communication;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.AsyncEx.Synchronous;
using PortalApi.Tests.Helpers;

namespace PortalApi.Tests
{
    [TestClass]
    public class PortalInteractionTests
    {
        PortalInteractionHelper helper;

        [TestInitialize]
        public async Task Init()
        {
            var exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });
            if (exitCode != 0)
            {
                throw new Exception($"Playwright exited with code {exitCode}");
            }

            helper = new PortalInteractionHelper();
            await helper.InitAsync();
        }

        [TestCleanup]
        public async Task Clean()
        {
            await helper.DisposeAsync();
        }

        [TestMethod]
        public async Task BrowserCanVisitBing()
        {
            await helper.VisitAsync("https://www.bing.com/maps", true);
        }

        [TestMethod]
        public async Task SignInToPortalSucceeds()
        {
            await helper.LogInAsync();
        }

        [TestMethod]
        public async Task PersonalPortalPagesRedirectToSignIn()
        {
            await helper.VisitAsync(PortalUris.PortalWeb_DiaryUri, true, $"{PortalUris.PortalWeb_LoginUri}/Account/Login");
            await helper.VisitAsync(PortalUris.PortalWeb_CalendarUri, true, $"{PortalUris.PortalWeb_LoginUri}/Account/Login");
            await helper.VisitAsync(PortalUris.PortalWeb_ConversationsUri, true, $"{PortalUris.PortalWeb_LoginUri}/Account/Login");
            await helper.VisitAsync(PortalUris.PortalWeb_MessageBoardsUri, true, $"{PortalUris.PortalWeb_LoginUri}/Account/Login");
            await helper.VisitAsync(PortalUris.PortalWeb_ResourcesUri, true, $"{PortalUris.PortalWeb_LoginUri}/Account/Login");
        }

        [TestMethod]
        public async Task PersonalPortalPagesVisibleAfterSignIn()
        {
            await helper.LogInAsync();
            await helper.VisitDiaryAsync();
            await helper.VisitCalendarAsync();
            await helper.VisitConversationsAsync();
            await helper.VisitMessageBoardsAsync();
            await helper.VisitResourcesAsync();
        }
    }
}
