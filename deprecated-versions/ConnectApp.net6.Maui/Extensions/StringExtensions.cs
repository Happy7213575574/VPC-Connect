using System;
namespace ConnectApp.Maui.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string RemoveDoubleSlashesPreserveHttps(this string url)
        {
            url = url.Replace("https://", "https:");
            while (url.Contains("//"))
            {
                url = url.Replace("//", "/");
            }
            url = url.Replace("https:", "https://");
            return url;
        }
    }
}
