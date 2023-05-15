using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using ConnectApp.AppLog;
using ConnectApp.DTO;
using Nito.AsyncEx;
using RestSharp;

namespace ConnectApp.Communication
{
    public class PortalApi
    {
        private RestClientOptions options;
        private RestClient client;
        private AppLogger log;
        private readonly AsyncLock _mutex = new AsyncLock(); // one thing at a time

        internal static readonly bool UseUserToken = true;

        public PortalApi(App app)
        {
            this.log = app != null ? app.Log.For(this) : new AppLogger(null, typeof(PortalApi).Name, false);
            options = new RestClientOptions(PortalUris.PortalApi_BaseUri);
            options.ThrowOnAnyError = false;
            if (PortalUris.OverrideTimeout != null) { options.Timeout = PortalUris.OverrideTimeout.Value; }
            client = new RestClient(options);
            client.AddDefaultHeader("API-Access", PortalUris.PortalApi_AccessCode);
            client.AddDefaultHeader("Access-Control-Allow-Origin", "*");

            // For situations when the stimulize (test) portal cert has expired ONLY:
            // AcceptAllCertsFromStimulize();
        }

        [Obsolete("Never do this in production! Do as little as possible in testing.")]
        private void AcceptAllCertsFromStimulize()
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicy) =>
            {
                if (sslPolicy == SslPolicyErrors.None)
                    return true;

                if (sslPolicy == SslPolicyErrors.RemoteCertificateChainErrors &&
                   ((HttpWebRequest)sender).RequestUri.Authority.Contains("stimulize.co.uk"))
                    return true;

                return false;
            };
        }

        public async Task<ServerResponse> SubmitPortalDeviceCheckAsync(string token, string uuid)
        {
            using (await _mutex.LockAsync())
            {
                var request = new RestRequest(PortalUris.DeviceCheckEndpoint, Method.Post);
                request.AddParameter("RegistrationId", token);
                request.AddParameter("UUID", uuid);
                LogRequest(request);
                var response = await client.ExecuteAsync(request);
                LogResponse(response);
                return ServerResponse.From(response);
            }
        }

        public async Task<UserTokenServerResponse> GetUserTokenAsync(string username, string password)
        {
            using (await _mutex.LockAsync())
            {
                var request = new RestRequest(PortalUris.UserTokenEndpoint, Method.Post);
                request.AddParameter("username", username);
                request.AddParameter("password", password);
                LogRequest(request);
                var response = await client.ExecuteAsync(request);
                LogResponse(response);
                return UserTokenServerResponse.From(response);
            }
        }

        public async Task<ServerResponse> SubmitUserTokenRegistrationAsync(string userToken, string pushToken, string deviceUuid, string deviceDescription)
        {
            using (await _mutex.LockAsync())
            {
                var request = new RestRequest(PortalUris.RegistrationEndpoint, Method.Post);
                request.AddParameter("DeviceType", deviceDescription);
                request.AddParameter("UUID", deviceUuid);
                request.AddParameter("RegistrationId", pushToken);
                request.AddParameter("UserToken", userToken);
                LogRequest(request);
                var response = await client.ExecuteAsync(request);
                LogResponse(response);
                return ServerResponse.From(response);
            }
        }

        [Obsolete("This method of registration is deprecated.")]
        internal async Task<ServerResponse> SubmitUsernamePasswordRegistrationAsync(string username, string password, string pushToken, string deviceUuid, string deviceDescription)
        {
            using (await _mutex.LockAsync())
            {
                var request = new RestRequest(PortalUris.RegistrationEndpoint, Method.Post);
                request.AddParameter("DeviceType", deviceDescription);
                request.AddParameter("UUID", deviceUuid);
                request.AddParameter("RegistrationId", pushToken);
                request.AddParameter("UserName", username);
                request.AddParameter("Password", password);
                LogRequest(request);
                var response = await client.ExecuteAsync(request);
                LogResponse(response);
                return ServerResponse.From(response);
            }
        }

        public async Task<ServerResponse> SubmitPortalDeregistrationAsync(string token, string uuid)
        {
            using (await _mutex.LockAsync())
            {
                var request = new RestRequest(PortalUris.DeregistrationEndpoint, Method.Post);
                request.AddParameter("RegistrationId", token);
                request.AddParameter("UUID", uuid);
                LogRequest(request);
                var response = await client.ExecuteAsync(request);
                LogResponse(response);
                return ServerResponse.From(response);
            }
        }

        private void LogRequest(RestRequest request)
        {
            log.Debug("RestClient.BuildUri: " + client.BuildUri(request).ToString(), false);
            log.Verbose("RestRequest.Method: " + request.Method, false);
            var paramsDict = request.Parameters.Where(p => p.Value != null).ToDictionary(p => p.Name, p => p.Value);
            var paramStrings = paramsDict.Keys.Select(k => " - " + k + ": " + paramsDict[k]);
            var parameters = string.Join("\n", paramStrings);
            if (paramStrings.Count() > 0)
            {
                log.Verbose("RestRequest.Parameters:\n" + parameters, true);
            }
            // request.Body removed from RestSharp
            //if (request.Body != null && request.Body.Value != null && !string.IsNullOrWhiteSpace(request.Body.Value.ToString()))
            //{
            //    log.Verbose("RestRequest.Body: " + request.Body?.Value?.ToString(), true);
            //}
        }

        private void LogResponse(RestResponse response)
        {
            log.Debug("RestResponse.ResponseStatus: " + response.ResponseStatus.ToString(), false);
            log.Debug("RestResponse.StatusCode: " + response.StatusCode.ToString() + " (" + (int)response.StatusCode + ")", false);
            if (!string.IsNullOrWhiteSpace(response.StatusDescription))
            {
                log.Debug("RestResponse.StatusDescription: " + response.StatusDescription, false);
            }
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                log.Verbose("RestResponse.Content: " + response.Content, true);
            }
        }
    }
}
