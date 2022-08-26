using System;
using System.Collections.Generic;
using Android.OS;
using ConnectApp.Analytics;
using ConnectApp.Droid;
using Firebase.Analytics;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidAnalyticsReporter))]
namespace ConnectApp.Droid
{
    public class AndroidAnalyticsReporter : IAnalyticsReporter
    {
        public void SendEvent(string eventId)
        {
            SendEvent(eventId, null);
        }

        public void SendEvent(string eventId, string paramName, string value)
        {
            SendEvent(eventId, new Dictionary<string, string>
            {
                {paramName, value}
            });
        }

        public void SendEvent(string eventId, IDictionary<string, string> parameters)
        {
            var firebaseAnalytics = FirebaseAnalytics.GetInstance(MainApplication.Context);

            if (parameters == null)
            {
                firebaseAnalytics.LogEvent(eventId, null);
                return;
            }

            var bundle = new Bundle();
            foreach (var param in parameters)
            {
                bundle.PutString(param.Key, param.Value);
            }

            firebaseAnalytics.LogEvent(eventId, bundle);
        }

        public void SendException(Exception e)
        {
            var eventId = "Exception";
            var details = new Dictionary<string, string>()
            {
                { "Type", e.GetType().FullName },
                { "Message", e.Message },
                { "StackTrace", e.StackTrace },                
            };
            SendEvent(eventId, details);
        }

        public void SendExceptionObject(object o)
        {
            var e = o as Exception;
            if (e != null)
            {
                SendException(e);
            }
            SendEvent("Error", "Message", o.ToString());
        }
    }
}