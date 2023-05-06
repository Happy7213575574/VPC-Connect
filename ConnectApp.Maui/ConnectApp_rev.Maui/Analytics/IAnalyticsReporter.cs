using System;
using System.Collections.Generic;

namespace ConnectApp.Maui.Analytics
{
    public interface IAnalyticsReporter
    {
        void SendEvent(string eventId);
        void SendEvent(string eventId, string paramName, string value);
        void SendEvent(string eventId, IDictionary<string, string> parameters);
        void SendException(Exception e);
        void SendExceptionObject(object o);
    }
}
