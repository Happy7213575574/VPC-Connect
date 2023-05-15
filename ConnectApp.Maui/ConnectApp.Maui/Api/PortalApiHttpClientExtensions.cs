using System;
namespace ConnectApp.Maui.Api
{
	public static class PortalApiHttpClientExtensions
	{
        [Obsolete]
        public static void SetHeaders(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("API-Access", SensitiveConstants.PortalApiAccessCode);
            client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
        }
    }
}

