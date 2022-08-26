using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NUnit.Framework;
using Xamarin.UITest;

namespace ConnectApp.AppTests.UI
{
    [TestFixture(Platform.Android)]
    public class NotificationDisplayTests : AbstractAppTests
    {
        protected override bool AlwaysFreshApp => true;

        public NotificationDisplayTests(Platform platform) : base(platform)
        {
        }

        [Test]
        public void AddTestNotificationButtonDisplaysInfoAboutNewNotification()
        {
            helper.NavigateToAppInfoPage();
            app.Tap("Add test notification");
            var texts = helper.GetPopupTexts();
            var title = texts.ElementAt(0);
            var text = texts.ElementAt(1);

            var pattern = "Notification \\d+";
            var regex = new Regex(pattern);
            Assert.True(regex.IsMatch(title));

            var notificationInfoType = new { title = "", body = "", sectionURL = "" };
            var json = JsonConvert.DeserializeAnonymousType(text, notificationInfoType);
            var notificationTitle = json.title;
            var notificationBody = json.body;
            var notificationUrl = json.sectionURL;

            Assert.IsNotEmpty(notificationTitle);
            Assert.IsNotEmpty(notificationBody);
            Assert.IsNotEmpty(notificationUrl);

            helper.ClickPopupOk();
            helper.CheckPopupGone();
        }

        [Test]
        public void NewTestNotificationsAppearOnHomePage()
        {
            // create 2 test notifications
            helper.NavigateToAppInfoPage();
            var notificationTexts_1 = helper.TapAddTestNotificationOnInfoPageGetNotification();
            var notificationTexts_2 = helper.TapAddTestNotificationOnInfoPageGetNotification();

            helper.NavigateTo("Home");

            // wait until the register box has appeared - this affects scrolling
            app.WaitForElement("Recent notifications");
            app.WaitForElement("Register");

            // scroll to just below the recent notifications section
            app.ScrollTo("Resources", "HomeScrollView");
            helper.AssertExactMatches(notificationTexts_2.ElementAt(0));
            helper.AssertExactMatches(notificationTexts_2.ElementAt(1));

            // ensure second item in the list is in view
            app.ScrollDownTo(notificationTexts_1.ElementAt(1), "RecentNotificationsList");
            helper.AssertExactMatches(notificationTexts_1.ElementAt(0));
            helper.AssertExactMatches(notificationTexts_1.ElementAt(1));
        }

        [Test]
        public void NewTestNotificationsAppearOnNotificationsPage()
        {
            // create 2 test notifications
            helper.NavigateToAppInfoPage();
            var notificationTexts_1 = helper.TapAddTestNotificationOnInfoPageGetNotification();
            var notificationTexts_2 = helper.TapAddTestNotificationOnInfoPageGetNotification();

            helper.NavigateTo("Notifications");

            // assumption: both items are at the top of the list
            // if this fails, implement scrolling on NotificationsList
            helper.AssertExactMatches(notificationTexts_2.ElementAt(0));
            helper.AssertExactMatches(notificationTexts_2.ElementAt(1));
            helper.AssertExactMatches(notificationTexts_1.ElementAt(0));
            helper.AssertExactMatches(notificationTexts_1.ElementAt(1));
        }

    }
}
