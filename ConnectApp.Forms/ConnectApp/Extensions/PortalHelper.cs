using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ConnectApp.AppLog;
using ConnectApp.Communication;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConnectApp.Extensions
{
    public class PortalHelper
    {
        public enum AnalyticsAction
        {
            OpenNotification
        }

        public static Uri PreparePortalAnalyticsUriQuery(Uri uri, AnalyticsAction action, object source, AppLogger log)
        {
            try
            {
                if (uri == null)
                {
                    log?.Warning("Unable to prepare querystring for null uri.", false);
                    return null;
                }
                log?.Debug($"Preparing querystring for uri: {uri.ToString()}", false);
                var uriStr = uri.ToString().RemoveDoubleSlashesPreserveHttps();
                var builder = new UriBuilder(uriStr);
                var query = HttpUtility.ParseQueryString(builder.Query ?? "");
                query["action"] = action.ToString();
                query["source"] = "org.vpc.connect";
                query["originator"] = source?.GetType().Name ?? "null";
                builder.Query = query.ToString();
                return builder.Uri;
            }
            catch (Exception e)
            {
                // if the parsing above fails, it's better to return the original Uri
                log?.Warning("Failed to append querystring to Uri: " + uri.ToString(), false, e);
                return uri;
            }
        }

        public static bool UriPermitted(Uri uri)
        {
            var permittedHosts = new []
            {
                new Uri(PortalUris.PortalWeb_BaseUri).Host.ToLower(),
                new Uri(PortalUris.PortalWeb_LoginUri).Host.ToLower(),
                new Uri("https://policerewired.org").Host.ToLower(),
                new Uri("https://www.google.com").Host.ToLower()
            };
            return permittedHosts.Contains(uri.Host.ToLower());
        }

        public static async Task<bool> VisitUriAsync(Uri uri)
        {
            return await Browser.OpenAsync(uri, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.AliceBlue,
                PreferredControlColor = Color.Violet
            });
        }

        [Obsolete("use VisitUriAsync to launch a browser")]
        public static async Task LaunchUriAsync(Uri uri)
        {
            await Launcher.OpenAsync(uri);
        }

    }
}
