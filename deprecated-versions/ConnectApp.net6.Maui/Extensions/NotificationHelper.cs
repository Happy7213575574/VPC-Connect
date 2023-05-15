using System;
using System.Collections.Generic;

namespace ConnectApp.Maui.Extensions
{
    public class NotificationHelper
    {
        public static string[] AdditionalDataPaths = new[]
        {
                "data.additionalData",
                "notification.additionalData",
                "aps.alert.additionalData",
                "aps.data.additionalData",
                "aps.additionalData",
                "additionalData"
        };

        private static string[] titleKeys = new[] { "title", "Title", "notice", "Notice", "aps.alert.title", "notification.title", "notification.Title" };
        private static string[] subtitleKeys = new[] { "subtitle", "Subtitle", "aps.alert.subtitle" };
        private static string[] bodyKeys = new[] { "body", "Body", "aps.alert.body", "aps.alert", "aps.alert.message", "aps.alert.text", "notification.body", "notification.Body" };
        private static string[] targetUrlKeys = new[] { "sectionURL", "notification.sectionURL", "data.sectionURL" };

        public static string GetTitle(IDictionary<string, string> notification)
        {
            return Search(notification, titleKeys) ?? "VPC portal notification";
        }

        public static string GetSubtitle(IDictionary<string, string> notification)
        {
            return Search(notification, subtitleKeys);
        }

        public static string GetMessage(IDictionary<string, string> notification)
        {
            return Search(notification, bodyKeys);
        }

        public static string GetTargetUrl(IDictionary<string,string> notification)
        {
            return Search(notification, targetUrlKeys);
        }

        private static string Search(IDictionary<string,string> notification, IEnumerable<string> options)
        {
            foreach (var key in options)
            {
                if (notification.ContainsKey(key))
                {
                    if (notification[key] is string)
                    {
                        return (string)notification[key];
                    }
                }
            }
            return null;
        }

    }
}
