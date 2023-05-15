using System;
using System.Collections.Generic;
using ConnectApp.Analytics;
using ConnectApp.Droid;
using Firebase.Crashlytics;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidCrashlyticsReporter))]
namespace ConnectApp.Droid
{
    public class AndroidCrashlyticsReporter : ICrashlyticsReporter
    {
        public void Init()
        {
            FirebaseCrashlytics.Instance.SetCrashlyticsCollectionEnabled(true);
            FirebaseCrashlytics.Instance.SendUnsentReports();
        }

        public void SimulateCrash()
        {
            var x = new Exception("User initiated Exception.");
            RecordException(x);
            throw x;
        }

        public void RecordExceptionObject(object o)
        {
            var e = o as Exception;
            if (e != null)
            {
                RecordException(e);
                return;
            }

            var j = o as Java.Lang.Exception;
            if (j != null)
            {
                FirebaseCrashlytics.Instance.RecordException(j);
                return;
            }

            RecordError(o.ToString());
        }

        public void RecordException(Exception e)
        {
            var msg = $"{e.GetType().Name}: {e.Message}\n{e.StackTrace}";
            var exception = new Java.Lang.Exception(msg);
            FirebaseCrashlytics.Instance.RecordException(exception);
        }

        public void RecordError(string error)
        {
            var exception = new Java.Lang.Exception(error);
            FirebaseCrashlytics.Instance.RecordException(exception);
        }
    }
}
