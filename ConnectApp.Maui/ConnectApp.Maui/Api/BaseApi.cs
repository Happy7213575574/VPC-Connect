using System;
using ConnectApp.Maui.Api.DTO;
using ConnectApp.Maui.AppLog;
using Nito.AsyncEx;

namespace ConnectApp.Maui.Api
{
	public abstract class BaseApi : IPortalApi
	{
		protected App app;
        protected AppLogger log;
        protected readonly AsyncLock _mutex = new AsyncLock(); // one thing at a time

        public BaseApi(App app)
		{
			this.app = app;
            this.log = app != null ? app.Log.For(this) : new AppLogger(null, typeof(PortalApiHttpClient).Name, false);
        }

        public abstract Task<ServerResponse> SubmitDeviceCheckAsync(string token, string uuid);
        public abstract Task<UserTokenServerResponse> GetUserTokenAsync(string username, string password);
        public abstract Task<ServerResponse> SubmitUserTokenRegistrationAsync(string userToken, string pushToken, string deviceUuid, string deviceDescription);
        public abstract Task<ServerResponse> SubmitPortalDeregistrationAsync(string token, string uuid);

        protected void LogRequest(HttpRequestMessage request)
        {
            log.Debug($"{request.Method} request to: {request.RequestUri}", false);
            var requestHeaderStrings = request.Headers.SelectMany(h => h.Value.Select(v => $" - {h.Key}: {v}"));
            var contentHeaderStrings = request.Content.Headers.SelectMany(h => h.Value.Select(v => $" - {h.Key}: {v}"));
            var headerStrings = new List<string>();
            headerStrings.AddRange(requestHeaderStrings);
            headerStrings.AddRange(contentHeaderStrings);
            if (headerStrings.Count() > 0)
            {
                var headers = string.Join("\n", headerStrings);
                log.Verbose("request Headers:\n" + headers, true);
            }
        }

        protected void LogResponse(ServerResponse response)
        {
            log.Debug($"{response.Code} response ({response.StatusDescription})", false);
            if (!string.IsNullOrWhiteSpace(response.RawContent))
            {
                log.Verbose("response.RawContent: " + response.RawContent, true);
            }
        }

    }
}

