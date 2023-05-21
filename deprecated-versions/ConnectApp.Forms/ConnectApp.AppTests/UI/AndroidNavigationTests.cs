using System;
using NUnit.Framework;
using Xamarin.UITest;

namespace ConnectApp.AppTests.UI
{
    [TestFixture(Platform.Android)]
    public class AndroidNavigationTests : AbstractAppTests
    {
        protected override bool AlwaysFreshApp => false;

        public AndroidNavigationTests(Platform platform) : base(platform)
        {
        }

        [Test]
        public void HomePageShowsRegisterButton()
        {
            helper.NavigateTo("Home");
            helper.FindText("VPC Connect", expected: 3);
            helper.AssertSingleMatch("Register");
        }

        [Test]
        public void RegisterButtonMovesToConnectPage()
        {
            helper.NavigateTo("Home");
            app.Tap("Register");
            helper.AssertSingleMatch("Real-time notifications");
        }
    }
}
