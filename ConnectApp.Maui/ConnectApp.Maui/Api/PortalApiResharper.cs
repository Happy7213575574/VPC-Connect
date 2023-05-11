using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ConnectApp.Maui.Api.DTO;
using ConnectApp.Maui.AppLog;

using Nito.AsyncEx;
using RestSharp;

namespace ConnectApp.Maui.Api
{
    public class PortalApiResharper : BaseApi, IPortalApi
    {
        private RestClientOptions options;
        private RestClient client;

        public PortalApiResharper(App app) : base(app)
        {
            options = new RestClientOptions(PortalUris.PortalApi_BaseUri);
            options.ThrowOnAnyError = false;
            options.RemoteCertificateValidationCallback += CustomValidationCallback;
            if (PortalUris.OverrideTimeout != null) { options.MaxTimeout = PortalUris.OverrideTimeout.Value; }
            client = new RestClient(options);
            client.AddDefaultHeader("API-Access", PortalUris.PortalApi_AccessCode);
            client.AddDefaultHeader("Access-Control-Allow-Origin", "*");
            // ServicePointManager.ServerCertificateValidationCallback += CustomValidationCallback;
            log.Debug("RestClient ready.", false);
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

        public override async Task<ServerResponse> SubmitDeviceCheckAsync(string token, string uuid)
        {
            log.Verbose("SubmitDeviceCheckAsync", false);
            using (await _mutex.LockAsync())
            {
                var request = new RestRequest(PortalUris.DeviceCheckEndpoint, Method.Post);
                request.AddParameter("RegistrationId", token);
                request.AddParameter("UUID", uuid);
                LogRequest(client, request);
                var response = await client.ExecuteAsync(request);
                var serverResponse = ServerResponse.From(response);
                LogResponse(serverResponse);
                return serverResponse;
            }
        }

        public override async Task<UserTokenServerResponse> GetUserTokenAsync(string username, string password)
        {
            log.Verbose("GetUserTokenAsync", false);
            using (await _mutex.LockAsync())
            {
                var request = new RestRequest(PortalUris.UserTokenEndpoint, Method.Post);
                request.AddParameter("username", username.Trim().ToLower()); // TODO: confirm all usernames should be lowercase
                request.AddParameter("password", password.Trim());
                LogRequest(client, request);
                var response = await client.ExecuteAsync(request);
                var serverResponse = UserTokenServerResponse.From(response);
                LogResponse(serverResponse);
                return serverResponse;
            }
        }

        public override async Task<ServerResponse> SubmitUserTokenRegistrationAsync(string userToken, string pushToken, string deviceUuid, string deviceDescription)
        {
            log.Verbose("SubmitUserTokenRegistrationAsync", false);
            using (await _mutex.LockAsync())
            {
                var request = new RestRequest(PortalUris.RegistrationEndpoint, Method.Post);
                request.AddParameter("DeviceType", deviceDescription);
                request.AddParameter("UUID", deviceUuid);
                request.AddParameter("RegistrationId", pushToken);
                request.AddParameter("UserToken", userToken);
                LogRequest(client, request);
                var response = await client.ExecuteAsync(request);
                var serverResponse = ServerResponse.From(response);
                LogResponse(serverResponse);
                return serverResponse;
            }
        }

        //[Obsolete("This method of registration is deprecated.")]
        //internal async Task<ServerResponse> SubmitUsernamePasswordRegistrationAsync(string username, string password, string pushToken, string deviceUuid, string deviceDescription)
        //{
        //    log.Verbose("SubmitUsernamePasswordRegistrationAsync", false);
        //    using (await _mutex.LockAsync())
        //    {
        //        var request = new RestRequest(PortalUris.RegistrationEndpoint, Method.Post);
        //        request.AddParameter("DeviceType", deviceDescription);
        //        request.AddParameter("UUID", deviceUuid);
        //        request.AddParameter("RegistrationId", pushToken);
        //        request.AddParameter("UserName", username);
        //        request.AddParameter("Password", password);
        //        LogRequest(client, request);
        //        var response = await client.ExecuteAsync(request);
        //        var serverResponse = ServerResponse.From(response);
        //        LogResponse(serverResponse);
        //        return serverResponse;
        //    }
        //}

        public override async Task<ServerResponse> SubmitPortalDeregistrationAsync(string token, string uuid)
        {
            log.Verbose("SubmitPortalDeregistrationAsync", false);
            using (await _mutex.LockAsync())
            {
                var request = new RestRequest(PortalUris.DeregistrationEndpoint, Method.Post);
                request.AddParameter("RegistrationId", token);
                request.AddParameter("UUID", uuid);
                LogRequest(client, request);
                var response = await client.ExecuteAsync(request);
                var serverResponse = ServerResponse.From(response);
                LogResponse(serverResponse);
                return serverResponse;
            }
        }

    }
}
