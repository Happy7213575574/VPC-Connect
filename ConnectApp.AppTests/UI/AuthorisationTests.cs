using System.Linq;
using ConnectApp.AppTests.Helper;
using NUnit.Framework;
using Xamarin.UITest;

namespace ConnectApp.AppTests.UI
{
    [TestFixture(Platform.Android)]
    public class AuthorisationTests : AbstractAppTests
    {
        protected override bool AlwaysFreshApp => true;

        public AuthorisationTests(Platform platform) : base(platform)
        {
        }

        [Test]
        public void CanSignInToPortalWithTestCredentials()
        {
            helper.SignInToPortal(
                AccountHelper.Username,
                AccountHelper.Password,
                true);
        }

        [Test]
        public void CannotSignInToPortalWithBadCredentials()
        {
            helper.SignInToPortal(AccountHelper.Username, "IncorrectPassword", false);
        }

        [Test]
        public void CanCancelSignOut()
        {
            helper.SignInToPortal(
                AccountHelper.Username,
                AccountHelper.Password,
                true);

            helper.NavigateTo("Connection");
            app.Tap("Sign out button");

            var texts = helper.GetPopupTexts();
            Assert.True(texts.ElementAt(0) == "Are you sure?");
            helper.ClickPopup("Cancel", "Cancel");
            helper.FindMarked("Sign out button");
        }

        [Test]
        public void CanSignOut()
        {
            helper.SignInToPortal(
                AccountHelper.Username,
                AccountHelper.Password,
                true);

            helper.SignOut();
        }

        [Test]
        public void SigningOutDeletesNotifications()
        {
            // sign in
            helper.SignInToPortal(
                AccountHelper.Username,
                AccountHelper.Password,
                true);

            // create 2 test notifications
            helper.NavigateToAppInfoPage();
            var notificationTexts_1 = helper.TapAddTestNotificationOnInfoPageGetNotification();
            var notificationTexts_2 = helper.TapAddTestNotificationOnInfoPageGetNotification();

            // ensure they exist
            helper.NavigateTo("Notifications");
            app.WaitForNoElement("No notifications received yet.");
            helper.AssertExactMatches(notificationTexts_2.ElementAt(0));
            helper.AssertExactMatches(notificationTexts_2.ElementAt(1));
            helper.AssertExactMatches(notificationTexts_1.ElementAt(0));
            helper.AssertExactMatches(notificationTexts_1.ElementAt(1));

            // sign out of the app
            helper.SignOut();

            // confirm notifications no longer displayed
            helper.NavigateTo("Notifications");
            helper.AssertExactMatches("No notifications received yet.");
            app.WaitForNoElement(notificationTexts_2.ElementAt(0));
            app.WaitForNoElement(notificationTexts_2.ElementAt(1));
            app.WaitForNoElement(notificationTexts_1.ElementAt(0));
            app.WaitForNoElement(notificationTexts_1.ElementAt(1));
        }

    }
}
