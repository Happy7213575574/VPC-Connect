using System;
using System.Threading;
using System.Threading.Tasks;
using ConnectApp.Communication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using PortalApi.Tests.Helpers;
using RestSharp;

namespace PortalApi.Tests
{
    [TestClass]
    public class PortalApiTests
    {
        private static readonly string DEVICE_UUID = "b9192502-d84e-4ac3-a639-9dd3496e0e86";
        //private static readonly string USER_TOKEN = "9ad515fa-04ba-4296-9f49-533316515f03";
        private static readonly string DEVICE_DESCRIPTION = "Android: test-device not-real";

        ConnectApp.Communication.PortalApi api;

        [TestInitialize]
        public void Init()
        {
            api = new ConnectApp.Communication.PortalApi(null);
        }

        [TestMethod]
        public void ApiClassDoesNotNeedApp()
        {
            Assert.IsNotNull(api);
        }

        [TestMethod]
        public async Task TokenEndpointAcceptsTestUser_Success()
        {
            var token = await api.GetUserTokenAsync(AccountHelper.Username, AccountHelper.Password);
            Assert.IsTrue(token.IsSuccess);
            Assert.IsFalse(string.IsNullOrWhiteSpace(token.UserToken));
        }

        [TestMethod]
        public async Task RegistrationEndpointAcceptsRegistrationByToken_Success()
        {
            var userToken = await api.GetUserTokenAsync(AccountHelper.Username, AccountHelper.Password);
            var pushToken = Guid.NewGuid();
            var deviceId = DEVICE_UUID;
            var deviceDescription = DEVICE_DESCRIPTION;

            var result = await api.SubmitUserTokenRegistrationAsync(
                userToken.UserToken,
                pushToken.ToString(),
                deviceId,
                deviceDescription);

            Assert.IsTrue(result.IsSuccess);

            var response = JObject.Parse(result.RawContent);
            Assert.IsFalse(response.GetValue("UserName").HasValues);
            Assert.IsFalse(response.GetValue("Password").HasValues);
            Assert.AreEqual(deviceId, response.GetValue("UUID"), "UUID mismatch");
            Assert.AreEqual(deviceDescription, response.GetValue("DeviceType"), "DeviceType mismatch");
            Assert.AreEqual(pushToken.ToString(), response.GetValue("RegistrationId"), "RegistrationId mismatch");
            Assert.AreEqual(userToken.UserToken, response.GetValue("UserToken"), "UserToken mismatch");
        }

        [TestMethod]
        public async Task DeregistrationEndpoint_Success()
        {
            var userToken = await api.GetUserTokenAsync(AccountHelper.Username, AccountHelper.Password);
            var pushToken = Guid.NewGuid();
            var deviceId = DEVICE_UUID;
            var deviceDescription = DEVICE_DESCRIPTION;

            var regResult = await api.SubmitUserTokenRegistrationAsync(
                userToken.UserToken,
                pushToken.ToString(),
                deviceId,
                deviceDescription);

            Assert.IsTrue(regResult.IsSuccess);

            Thread.Sleep(3);

            var deregResult = await api.SubmitPortalDeregistrationAsync(
                userToken.UserToken,
                deviceId);

            Assert.IsTrue(deregResult.IsSuccess);
        }


    }
}
