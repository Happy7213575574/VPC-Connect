using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Plugin.FirebasePushNotification;
using UIKit;

[assembly: Preserve(typeof(Firebase.Analytics.Analytics), AllMembers = true)]
namespace ConnectApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //#if ENABLE_TEST_CLOUD
              Xamarin.Calabash.Start();
            //#endif

            // added to permit use of SwipeView
            global::Xamarin.Forms.Forms.SetFlags("SwipeView_Experimental");

            global::Xamarin.Forms.Forms.Init();

            // FA: configure analytics
            var typeA = typeof(Firebase.Analytics.Analytics);
            var typeC = typeof(Firebase.Crashlytics.Crashlytics);
            Firebase.Core.App.Configure();

            LoadApplication(new App());

            // FCM: init the push notification manager
            FirebasePushNotificationManager.Initialize(options, true);

            return base.FinishedLaunching(app, options);
        }

        /// <summary>
        /// FCM: passes the device token back to FCM
        /// </summary>
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        /// <summary>
        /// FCM: passes the error back to FCM
        /// </summary>
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
        }

        /// <summary>
        /// FCM: passes push notification back to FCM.
        /// To receive notifications in foregroung on iOS 9 and below.
        /// To receive notifications in background in any iOS version.
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userInfo"></param>
        /// <param name="completionHandler"></param>
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.

            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }
    }
}
