using Foundation;
using Microsoft.Win32;
using UIKit;
using UserNotifications;

namespace ConnectApp.Maui;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    //[Export("applicationDidFinishLaunching:")]
    //public void FinishedLaunching(UIApplication application)
    //{
    //    base.FinishedLaunching();
    //}

    //[Export("application:didFinishLaunchingWithOptions:")]
    //public bool FinishedLaunching(UIKit.UIApplication application, NSDictionary launchOptions)
    //{
    //    return base.FinishedLaunching(application, launchOptions);
    //}

    //Firebase.Core.App.Configure();

    //// Register your app for remote notifications.
    //if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
    //    {

    //        // For iOS 10 display notification (sent via APNS)
    //        UNUserNotificationCenter.Current.Delegate = this;

    //        var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
    //        UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
    //            Console.WriteLine(granted);
    //        });
    //    }
    //    else
    //    {
    //        // iOS 9 or before
    //        var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
    //        var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
    //        UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
    //    }

    //UIApplication.SharedApplication.RegisterForRemoteNotifications();

}

