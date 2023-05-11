using System;
using ConnectApp.Maui.Api.DTO;
using ConnectApp.Maui.AppLog;
using Nito.AsyncEx;
using RestSharp;

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
            this.log = app != null ? app.Log.For(this) : new AppLogger(null, typeof(PortalApiResharper).Name, false);
        }

        public bool UseUserToken => true;

        public abstract Task<ServerResponse> SubmitDeviceCheckAsync(string token, string uuid);
        public abstract Task<UserTokenServerResponse> GetUserTokenAsync(string username, string password);
        public abstract Task<ServerResponse> SubmitUserTokenRegistrationAsync(string userToken, string pushToken, string deviceUuid, string deviceDescription);
        public abstract Task<ServerResponse> SubmitPortalDeregistrationAsync(string token, string uuid);

        protected void LogRequest(RestClient client, RestRequest request)
        {
            log.Debug("RestClient.BuildUri: " + client.BuildUri(request).ToString(), false);
            log.Verbose("RestRequest.Method: " + request.Method, false);

            var paramsDict = request.Parameters.Where(p => p.Value != null).ToDictionary(p => p.Name, p => p.Value);
            var paramStrings = paramsDict.Keys.Select(k => " - " + k + ": " + paramsDict[k]);
            if (paramStrings.Count() > 0)
            {
                var parameters = string.Join("\n", paramStrings);
                log.Verbose("RestRequest.Parameters:\n" + parameters, true);
            }
        }

        protected void LogRequest(HttpRequestMessage request)
        {
            log.Debug("request.RequestUri: " + request.RequestUri, false);
            log.Verbose("request.Method: " + request.Method, false);

            var headerStrings = request.Content.Headers.SelectMany(h => h.Value.Select(v => $" - {h.Key}: {v}"));
            if (headerStrings.Count() > 0)
            {
                var headers = string.Join("\n", headerStrings);
                log.Verbose("request.Headers:\n" + headers, true);
            }
        }

        protected void LogResponse(ServerResponse response)
        {
            log.Debug("response.Code: " + response.Code, false);
            log.Debug("response.StatusDescription: " + response.StatusDescription.ToString(), false);
            if (!string.IsNullOrWhiteSpace(response.RawContent))
            {
                log.Verbose("response.RawContent: " + response.RawContent, true);
            }
        }

    }
}

