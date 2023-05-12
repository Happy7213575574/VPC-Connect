using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ConnectApp.Maui.Api.DTO;
using ConnectApp.Maui.AppLog;
using Newtonsoft.Json;
using Nito.AsyncEx;
using RestSharp;

namespace ConnectApp.Maui.Api
{
    public class PortalApiHttpClient : BaseApi, IPortalApi
    {
        HttpClient httpclient;

        public PortalApiHttpClient(App app) : base(app)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = CustomValidationCallback;
            var httpclient = new HttpClient(handler);
            httpclient.DefaultRequestHeaders.Add("API-Access", PortalUris.PortalApi_AccessCode);
            httpclient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
            // ServicePointManager.ServerCertificateValidationCallback += CustomValidationCallback;
            log.Debug("HttpClient ready.", false);
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

        private Uri ConstructUri(string endpoint)
            => (endpoint.StartsWith("/") || PortalUris.PortalApi_BaseUri.EndsWith("/"))
                ? new Uri($"{PortalUris.PortalApi_BaseUri}{endpoint}")
                : new Uri($"{PortalUris.PortalApi_BaseUri}/{endpoint}");

        private StringContent JsonContent(IDictionary<string, string> values)
            => new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

        public override async Task<ServerResponse> SubmitDeviceCheckAsync(string token, string uuid)
        {
            if (string.IsNullOrWhiteSpace(token)) { return ServerResponse.From(null, "Token not present - no check conducted."); }
            log.Verbose("SubmitDeviceCheckAsync", false);
            using (await _mutex.LockAsync())
            {
                var uri = ConstructUri(PortalUris.DeviceCheckEndpoint);
                var json = JsonContent(new Dictionary<string, string>() { { "RegistrationId", token }, { "UUID", uuid } });
                HttpResponseMessage response = await httpclient.PostAsync(uri, json);
                LogRequest(response.RequestMessage);
                var serverResponse = await ServerResponse.FromAsync(response);
                LogResponse(serverResponse);
                return serverResponse;
            }
        }

        public override async Task<UserTokenServerResponse> GetUserTokenAsync(string username, string password)
        {
            log.Verbose("GetUserTokenAsync", false);
            using (await _mutex.LockAsync())
            {
                var uri = ConstructUri(PortalUris.UserTokenEndpoint);
                var json = JsonContent(new Dictionary<string, string>()
                {
                    { "username", username.Trim().ToLower() },
                    { "password", password.Trim() }
                });
                var response = await httpclient.PostAsync(uri, json);
                LogRequest(response.RequestMessage);
                var serverResponse = await UserTokenServerResponse.FromAsync(response);
                LogResponse(serverResponse);
                return serverResponse;

            }
        }

        public override async Task<ServerResponse> SubmitUserTokenRegistrationAsync(string userToken, string pushToken, string deviceUuid, string deviceDescription)
        {
            log.Verbose("SubmitUserTokenRegistrationAsync", false);
            using (await _mutex.LockAsync())
            {
                var uri = ConstructUri(PortalUris.RegistrationEndpoint);
                var json = JsonContent(new Dictionary<string, string>()
                {
                    { "DeviceType", deviceDescription },
                    { "UUID", deviceUuid },
                    { "RegistrationId", pushToken },
                    { "UserToken", userToken }
                });
                var response = await httpclient.PostAsync(uri, json);
                LogRequest(response.RequestMessage);
                var serverResponse = await UserTokenServerResponse.FromAsync(response);
                LogResponse(serverResponse);
                return serverResponse;
            }
        }

        public override async Task<ServerResponse> SubmitPortalDeregistrationAsync(string token, string uuid)
        {
            log.Verbose("SubmitPortalDeregistrationAsync", false);
            using (await _mutex.LockAsync())
            {
                var uri = ConstructUri(PortalUris.DeregistrationEndpoint);
                var json = JsonContent(new Dictionary<string, string>()
                {
                    { "RegistrationId", token },
                    { "UUID", uuid }
                });
                var response = await httpclient.PostAsync(uri, json);
                LogRequest(response.RequestMessage);
                var serverResponse = await UserTokenServerResponse.FromAsync(response);
                LogResponse(serverResponse);
                return serverResponse;
            }
        }
    }
}
