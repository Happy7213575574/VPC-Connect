using System.Net;
using System.Net.Security;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using ConnectApp.Maui.Api.DTO;
using ConnectApp.Maui.AppLog;

using Nito.AsyncEx;
using RestSharp;

namespace ConnectApp.Maui.Api
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
            options.RemoteCertificateValidationCallback = CustomValidationCallback;
            if (PortalUris.OverrideTimeout != null) { options.MaxTimeout = PortalUris.OverrideTimeout.Value; }
            client = new RestClient(options);
            client.AddDefaultHeader("API-Access", PortalUris.PortalApi_AccessCode);
            client.AddDefaultHeader("Access-Control-Allow-Origin", "*");
        }

        private string debugAcceptDomain = SensitiveConstants.PortalApiTrustDomain;
        private bool CustomValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicy)
        {
            log.Debug($"Incoming sslPolicy = {sslPolicy}", true);
            if (sslPolicy == SslPolicyErrors.None)
            {
                log.Debug("Certificate accepted: SslPolicyErrors.None", true);
                return true;
            }

            var incomingDomain = ((HttpRequestMessage)sender).RequestUri.Authority;
            log.Debug($"Incoming domain:       {incomingDomain}", true);
            log.Debug($"Incoming cert subject: {certificate.Subject}", true);
            log.Debug($"Incoming cert issuer:  {certificate.Issuer}", true);
            log.Debug($"Incoming cert hash:    {certificate.GetCertHashString()}", true);
            log.Debug($"Incoming chain:        {chain.ChainElements.Count()} elements", true);

            if (sslPolicy == SslPolicyErrors.RemoteCertificateChainErrors &&
               (debugAcceptDomain == null || incomingDomain.Contains(debugAcceptDomain)))
            {
                log.Debug("Validating certifiate against known roots...", true);
                var ok = CertificateValidator.ValidateCertificateAgainstKnownRoots(certificate);
                log.Debug($"Certificate ok: {ok}", true);
                return ok;
            }

            // something else is an issue
            return false;
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
