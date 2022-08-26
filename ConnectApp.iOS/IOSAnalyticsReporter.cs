using System;
using System.Collections.Generic;
using ConnectApp.Analytics;
using ConnectApp.iOS;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOSAnalyticsReporter))]
namespace ConnectApp.iOS
{
    public class IOSAnalyticsReporter : IAnalyticsReporter
    {
        public void SendEvent(string eventId)
        {
            SendEvent(eventId, (IDictionary<string, string>)null);
        }

        public void SendEvent(string eventId, string paramName, string value)
        {
            SendEvent(eventId, new Dictionary<string, string>
            {
                { paramName, value }
            });
        }

        public void SendEvent(string eventId, IDictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                Firebase.Analytics.Analytics.LogEvent(eventId, (Dictionary<object, object>)null);
                return;
            }

            var keys = new List<NSString>();
            var values = new List<NSString>();
            foreach (var item in parameters)
            {
                keys.Add(new NSString(item.Key));
                values.Add(new NSString(item.Value));
            }

            var parametersDictionary = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
            Firebase.Analytics.Analytics.LogEvent(eventId, parametersDictionary);
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
