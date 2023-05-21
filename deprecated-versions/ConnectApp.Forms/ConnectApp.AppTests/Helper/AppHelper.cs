using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace ConnectApp.AppTests.Helper
{
    public class AppHelper
    {
        IApp app;
        Platform platform;

        public AppHelper(IApp app, Platform platform)
        {
            this.app = app;
            this.platform = platform;
        }

        public string MenuAutomationId
        {
            get
            {
                switch (platform)
                {
                    case Platform.Android:
                        return "OK";
                    case Platform.iOS:
                        return "MenuX";
                    default:
                        throw new NotSupportedException("Unsupported platform: " + platform.ToString());
                }
            }
        }

        public void SwipeMenuOpen()
        {
            //app.Tap(MenuAutomationId);
            app.SwipeLeftToRight(0.99, 1000);
        }

        public void NavigateTo(string menuEntry)
        {
            SwipeMenuOpen();
            app.Tap("View: " + menuEntry);
        }

        public void NavigateToAppInfoPage()
        {
            NavigateTo("Home");

            for (int i = 0; i < 20; i++)
                app.Tap("ImageBtn: MainLogo");
        }

        public void AssertExactMatches(string match)
        {
            FindText(match, true, null);
        }

        public void AssertPartialMatches(string match)
        {
            FindText(match, false, null);
        }

        public void AssertSingleMatch(string match)
        {
            FindText(match, true, 1);
        }

        public IEnumerable<AppResult> FindByAutomationId(string match)
        {
            var elements = app.WaitForElement(match, timeout: TimeSpan.FromSeconds(5));
            Assert.IsTrue(elements.Count() > 0);
            return elements;
        }

        public IEnumerable<AppResult> FindText(string match, bool exact = true, int? expected = null)
        {
            var elements = app.WaitForElement(e => exact
                    ? e.All().Property("text").Like(match)
                    : e.All().Property("text").Contains(match),
                timeout: TimeSpan.FromSeconds(5));

            if (expected != null)
            {
                Assert.AreEqual(expected, elements.Count());
            }
            else
            {
                Assert.IsTrue(elements.Count() > 0);
            }
            return elements;
        }

        public IEnumerable<AppResult> FindMarked(string match)
        {
            var elements = app.WaitForElement(match, timeout: TimeSpan.FromSeconds(5));
            Assert.IsTrue(elements.Count() == 1);
            return elements;
        }

        // Popups, see: https://forums.xamarin.com/discussion/90847/handling-pop-ups-with-xamarin-uitest

        public AppResult[] GetPopup()
        {
            switch (platform)
            {
                case Platform.Android:
                    return app.WaitForElement(c => c.Class("AlertDialogLayout"));

                case Platform.iOS:
                    return app.WaitForElement(c => c.ClassFull("_UIAlertControllerView"));

                default:
                    throw new NotSupportedException("Unsupported platform: " + platform.ToString());
            }
        }

        public void CheckPopupGone()
        {
            switch (platform)
            {
                case Platform.Android:
                    app.WaitForNoElement(c => c.Class("AlertDialogLayout"));
                    break;

                case Platform.iOS:
                    app.WaitForNoElement(c => c.ClassFull("_UIAlertControllerView"));
                    break;

                default:
                    throw new NotSupportedException("Unsupported platform: " + platform.ToString());
            }
        }

        public IEnumerable<string> GetPopupTexts()
        {
            switch (platform)
            {
                case Platform.Android:
                    var title = app.WaitForElement(c => c.Class("AlertDialogLayout").Descendant("DialogTitle")).Select(e => e.Text).FirstOrDefault();
                    var text = app.WaitForElement(c => c.Class("AlertDialogLayout").Descendant("AppCompatTextView")).Select(e => e.Text).FirstOrDefault();
                    return new[] { title, text };

                case Platform.iOS:
                    return app.WaitForElement(c => c.ClassFull("_UIAlertControllerView").Descendant("UILabel")).Take(2).Select(e => e.Text);

                default:
                    throw new NotSupportedException("Unsupported platform: " + platform.ToString());
            }
        }

        public void Tap(AppResult element)
        {
            app.TapCoordinates(element.Rect.CenterX, element.Rect.CenterY);
        }

        public void ClickPopupOk()
        {
            ClickPopup("Ok", "OK");
        }

        public void ClickPopup(string btnDroid, string btniOS)
        {
            switch (platform)
            {
                case Platform.Android:
                    var buttonDroid = app.WaitForElement(c => c.Marked(btnDroid).Class("AppCompatButton"));
                    Tap(buttonDroid.Single());
                    break;

                case Platform.iOS:
                    var buttoniOS = app.WaitForElement(c => c.Marked(btniOS).Class("UILabel"));
                    Tap(buttoniOS.Single()); // TODO: capitalisation of OK unknown for iOS version - yet
                    break;

                default:
                    throw new NotSupportedException("Unsupported platform: " + platform.ToString());
            }
        }

        public IEnumerable<string> TapAddTestNotificationOnInfoPageGetNotification()
        {
            app.Tap("Add test notification");

            var texts = GetPopupTexts();
            var text = texts.ElementAt(1);
            var notificationInfoType = new { title = "", body = "", sectionURL = "" };
            var json = JsonConvert.DeserializeAnonymousType(text, notificationInfoType);
            var notificationTitle = json.title;
            var notificationBody = json.body;

            Assert.IsNotEmpty(notificationTitle);
            Assert.IsNotEmpty(notificationBody);

            ClickPopupOk();
            CheckPopupGone();

            return new[] { notificationTitle, notificationBody };
        }

        public void SignInToPortal(string username, string password, bool expectSuccess)
        {
            NavigateTo("Connection");
            AssertExactMatches("Login");

            app.EnterText("input_Username", username);
            app.EnterText("input_Password", password);
            app.Tap("Login button");

            if (expectSuccess)
            {
                AssertExactMatches("Registered for notifications.");
                AssertExactMatches("Registration complete.");
                FindMarked("Home button");
                FindMarked("Sign out button");

                app.Tap("Home button");
                AssertExactMatches("VPC Connect");
                AssertExactMatches("cadet communications centre");
                AssertExactMatches("Registered for notifications.");
            }
            else
            {
                AssertExactMatches("Registration error.");
                FindMarked("Login button");
                app.WaitForNoElement("Home button");
                app.WaitForNoElement("Sign out button");
                NavigateTo("Home");
                AssertSingleMatch("Register");
            }
        }

        public void SignOut()
        {
            NavigateTo("Connection");

            app.Tap("Sign out button");

            var texts = GetPopupTexts();
            Assert.True(texts.ElementAt(0) == "Are you sure?");
            ClickPopup("Sign out", "Sign out");

            app.WaitForNoElement("Sign out button");
            AssertExactMatches("Signed out.");
        }

        public Dictionary<string,string> GetAppIdentifiers()
        {
            NavigateToAppInfoPage();
            AssertSingleMatch("Push token");

            // read the push token, assert not empty
            var token = app.Query(q => q.Marked("DeviceSummary")).Select(e => e.Text).Single();
            // TODO: push token can be empty during tests :'(
            Assert.IsNotEmpty(token);

            AssertSingleMatch("Device");

            // read the device and UUID text
            var deviceAndUuid = app.Query(q => q.Marked("DeviceSummary")).Select(e => e.Text).Single();
            Assert.IsNotEmpty(deviceAndUuid);

            // and split by line, check values
            var parts = deviceAndUuid.Split('\n');
            Assert.True(parts.Count() == 2);
            Assert.True(parts[0].StartsWith("Device: "));
            Assert.True(parts[1].StartsWith("UUID: "));

            var deviceStr = parts[0].Substring("Device: ".Length);
            Assert.True(deviceStr.StartsWith("Android: ") || deviceStr.StartsWith("iOS: "));

            var uuidStr = parts[1].Substring("UUID: ".Length);
            var uuid = Guid.Parse(uuidStr);
            Assert.NotNull(uuid);

            var appSummary = app.Query(q => q.Marked("AppSummary")).Select(e => e.Text).Single();
            Assert.IsNotEmpty(appSummary);

            var environment = app.Query(q => q.Marked("Environment")).Select(e => e.Text).Single();
            Assert.IsNotEmpty(environment);

            var portalApiBaseUri = app.Query(q => q.Marked("PortalApiBaseUri")).Select(e => e.Text).Single();
            Assert.IsNotEmpty(portalApiBaseUri);

            var portalSignInUri = app.Query(q => q.Marked("PortalSignInUri")).Select(e => e.Text).Single();
            Assert.IsNotEmpty(portalSignInUri);

            return new Dictionary<string, string>()
            {
                { "summary", appSummary }, // App version: 1.9\nApp build: 9
                { "environment", environment }, // DEBUG build, TEST portal
                { "api_uri", portalApiBaseUri }, // <uri>
                { "signin_uri", portalSignInUri }, // <uri>
                { "token", token }, // long string
                { "device", deviceStr }, // (Android|iOS): stuff...
                { "uuid", uuidStr }, // <uuid>
            };
        }

    }
}
