using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ConnectApp.Maui.Api.DTO;
using Newtonsoft.Json;

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
            httpclient = new HttpClient(handler);
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
               (debugAcceptDomain == null || incomingDomain.ToLower().Contains(debugAcceptDomain.ToLower())))
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

        private HttpRequestMessage PostMessage(Uri uri, StringContent content)
        {
            //content.Headers.Add("API-Access", PortalUris.PortalApi_AccessCode);
            //content.Headers.Add("Access-Control-Allow-Origin", "*");

            var msg = new HttpRequestMessage()
            {
                RequestUri = uri,
                Method = HttpMethod.Post,
                Content = content,
                Headers =
                {
                    { "API-Access", SensitiveConstants.PortalApiAccessCode },
                    { "Access-Control-Allow-Origin", "*" }
                }
            };

            log.Info($"Access code = {SensitiveConstants.PortalApiAccessCode}", false); // TODO: remove

            LogRequest(msg);

            return msg;
        }

        public override async Task<ServerResponse> SubmitDeviceCheckAsync(string token, string uuid)
        {
            if (string.IsNullOrWhiteSpace(token)) { return ServerResponse.From(null, "Token not present - no check conducted."); }
            log.Verbose("SubmitDeviceCheckAsync", false);
            using (await _mutex.LockAsync())
            {
                var uri = ConstructUri(PortalUris.DeviceCheckEndpoint);
                var json = JsonContent(new Dictionary<string, string>() { { "RegistrationId", token }, { "UUID", uuid } });
                var message = PostMessage(uri, json);
                //httpclient.SetHeaders();
                //HttpResponseMessage response = await httpclient.PostAsync(uri, json);
                HttpResponseMessage response = await httpclient.SendAsync(message);
                var serverResponse = await ServerResponse.FromAsync(response);
                LogResponse(serverResponse);
                return serverResponse;
            }
        }

        public override async Task<UserTokenServerResponse> GetUserTokenAsync(string username, string password)
        {
            log.Verbose("GetUserTokenAsync", false);
            log.Info($"GetUserTokenAsync username: {username}, password: {password}", false); // TODO: REMOVE THIS
            using (await _mutex.LockAsync())
            {
                var uri = ConstructUri(PortalUris.UserTokenEndpoint);
                var json = JsonContent(new Dictionary<string, string>()
                {
                    { "username", username.Trim().ToLower() },
                    { "password", password.Trim() }
                });
                var message = PostMessage(uri, json);
                //httpclient.SetHeaders();
                //HttpResponseMessage response = await httpclient.PostAsync(uri, json);
                HttpResponseMessage response = await httpclient.SendAsync(message);
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
                var message = PostMessage(uri, json);
                //httpclient.SetHeaders();
                //HttpResponseMessage response = await httpclient.PostAsync(uri, json);
                HttpResponseMessage response = await httpclient.SendAsync(message);
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
                var message = PostMessage(uri, json);
                //httpclient.SetHeaders();
                //HttpResponseMessage response = await httpclient.PostAsync(uri, json);
                HttpResponseMessage response = await httpclient.SendAsync(message);
                LogRequest(response.RequestMessage);
                var serverResponse = await UserTokenServerResponse.FromAsync(response);
                LogResponse(serverResponse);
                return serverResponse;
            }
        }
    }
}
