using System.Threading.Tasks;
using ConnectApp.Communication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortalApi.Tests.Helpers;

namespace PortalApi.Tests
{
    [TestClass]
    public class SimplePortalPageTests
    {
        [TestMethod]
        public async Task StaticWebUrlsAvailable()
        {
            await SimplePortalWebHelper.AssertWebPageAvailableAsync(PortalUris.WebsiteUri, null);
            await SimplePortalWebHelper.AssertWebPageAvailableAsync(PortalUris.PrivacyPolicyUri, false);
            await SimplePortalWebHelper.AssertWebPageAvailableAsync(PortalUris.PortalWeb_LoginUri, true);
        }

        [TestMethod]
        public async Task PersonalPortalUrlsRedirectToSignIn()
        {
            await SimplePortalWebHelper.AssertWebPageAvailableAsync(PortalUris.PortalWeb_DeepLinkToEventUri, true);
            await SimplePortalWebHelper.AssertWebPageAvailableAsync(PortalUris.PortalWeb_DiaryUri, true);
            await SimplePortalWebHelper.AssertWebPageAvailableAsync(PortalUris.PortalWeb_CalendarUri, true);
            await SimplePortalWebHelper.AssertWebPageAvailableAsync(PortalUris.PortalWeb_ConversationsUri, true);
            await SimplePortalWebHelper.AssertWebPageAvailableAsync(PortalUris.PortalWeb_MessageBoardsUri, true);
            await SimplePortalWebHelper.AssertWebPageAvailableAsync(PortalUris.PortalWeb_ResourcesUri, true);
        }

    }
}
