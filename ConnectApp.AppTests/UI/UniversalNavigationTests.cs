using System.Linq;
using System.Threading;
using NUnit.Framework;
using Xamarin.UITest;

namespace ConnectApp.AppTests.UI
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class UniversalNavigationTests : AbstractAppTests
    {
        protected override bool AlwaysFreshApp => false;

        public UniversalNavigationTests(Platform platform) : base(platform)
        {
        }

        [Test]
        public void HomePageIsDisplayed()
        {
            helper.NavigateTo("Home");
            app.Screenshot("Home screen");
            helper.FindText("VPC Connect", expected: 3);
            helper.FindText("News");
            helper.FindText("Safeguarding", expected: 2);
            helper.FindText("Visit the portal");
            helper.FindText("Diary");
            helper.FindText("Calendar");
            helper.FindText("Conversations");
            helper.FindText("Message boards");
            helper.FindText("Resources");
            helper.FindText("About");
        }

        [Test]
        public void AppInfoPageIsDisplayed()
        {
            helper.NavigateToAppInfoPage();
            app.Screenshot("app info");

            helper.AssertSingleMatch("Application");
            helper.AssertSingleMatch("Push token");
            helper.AssertSingleMatch("Device");
            helper.AssertSingleMatch("Debug tools");
        }

        [Test]
        public void ConnectionPageIsDisplayed()
        {
            helper.NavigateTo("Connection");
            app.Screenshot("connection");
            helper.AssertSingleMatch("Real-time notifications");
        }

        [Test]
        public void NotificationsPageIsDisplayed()
        {
            helper.NavigateTo("Notifications");
            app.Screenshot("notifications");

            helper.AssertSingleMatch("No notifications received yet.");
        }

        [Test]
        public void NewsFeedsPageIsDisplayed()
        {
            helper.NavigateTo("News feeds");
            app.Screenshot("newsfeeds");

            helper.AssertSingleMatch("National VPC");
            helper.AssertSingleMatch("All Cadets");
            helper.AssertSingleMatch("Mini Police");
        }

        [Test]
        public void SafeguardingPageIsDisplayed()
        {
            helper.NavigateTo("Safeguarding");
            app.Screenshot("safeguarding");

            helper.FindText("Safeguarding", expected: 3);
            helper.AssertSingleMatch("Report an issue");
            helper.AssertSingleMatch("NSPCC");
            helper.AssertSingleMatch("IOPC");
            helper.AssertSingleMatch("Statement");
        }

        [Test]
        public void CanFindMenu()
        {
            helper.FindByAutomationId(helper.MenuAutomationId);
        }

        [Test]
        public void MenuToggles()
        {
            helper.SwipeMenuOpen();
            helper.FindByAutomationId("View: Home");
            helper.FindByAutomationId("View: Connection");
            helper.FindByAutomationId("View: Notifications");
            helper.FindByAutomationId("View: News feeds");
            helper.FindByAutomationId("View: Safeguarding");

            app.Tap("View: Home");
            app.WaitForNoElement("View: Home");
        }
    }
}
