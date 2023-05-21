using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Plugin.FirebasePushNotification;

namespace ConnectApp.Droid
{
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            //FCM: Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "VPCConnect";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "PortalNotifications";
            }

#if DEBUG
            //FCM: If in debug mode, you should reset the token each time.
            FirebasePushNotificationManager.Initialize(this, true);
#else
              FirebasePushNotificationManager.Initialize(this, false);
#endif

            base.OnCreate();
        }
    }
}
