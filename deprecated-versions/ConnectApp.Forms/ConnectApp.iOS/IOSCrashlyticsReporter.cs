using System;
using ConnectApp.Analytics;
using ConnectApp.iOS;
using Firebase.Crashlytics;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOSCrashlyticsReporter))]
namespace ConnectApp.iOS
{
    public class IOSCrashlyticsReporter : ICrashlyticsReporter
    {
        public void Init()
        {
            Crashlytics.SharedInstance.SetCrashlyticsCollectionEnabled(true);
            Crashlytics.SharedInstance.SendUnsentReports();
        }

        public void RecordError(string error)
        {
            ExceptionModel model = new ExceptionModel("ConnectApp.iOS.Error", error);
            Crashlytics.SharedInstance.RecordExceptionModel(model);
        }

        public void RecordException(Exception e)
        {
            var msg = $"{e.GetType().Name}: {e.Message}\n{e.StackTrace}";
            ExceptionModel model = new ExceptionModel(e.GetType().FullName, msg);
            Crashlytics.SharedInstance.RecordExceptionModel(model);
        }

        public void RecordExceptionObject(object o)
        {
            var e = o as Exception;
            if (e != null)
            {
                RecordException(e);
                return;
            }
            RecordError(o.ToString());
        }

        public void SimulateCrash()
        {
            var x = new Exception("User initiated crash.");
            RecordException(x);
            throw x;
        }
    }
}
