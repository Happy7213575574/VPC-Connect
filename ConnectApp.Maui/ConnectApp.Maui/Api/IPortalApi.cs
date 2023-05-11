using System;
using ConnectApp.Maui.Api.DTO;

namespace ConnectApp.Maui.Api
{
	public interface IPortalApi
	{
        /// <summary>
        /// If true, use the user token for API calls (recommended), otherwise use username/password combo (not recommended).
        /// </summary>
        bool UseUserToken { get; }

        /// <summary>
        /// Checks to see if the device is currently registered
        /// </summary>
        /// <param name="token"></param>
        /// <param name="uuid"></param>
        /// <returns>200 if registered, 404 if not found</returns>
        Task<ServerResponse> SubmitDeviceCheckAsync(string token, string uuid);

        /// <summary>
        /// Exchanges username and password for a user token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>200 and a plain string containing the token if ok, 404 if the username or password not found</returns>
        Task<UserTokenServerResponse> GetUserTokenAsync(string username, string password);

        /// <summary>
        /// Registers a device to receive push notifications for a user
        /// </summary>
        /// <param name="userToken">user token obtained from portal</param>
        /// <param name="pushToken">Push token provided by Firebase SDK</param>
        /// <param name="deviceUuid">Unique Id of the device</param>
        /// <param name="deviceDescription">Begin device description with "Android:" or "iOS:" see: ConnnectDevice.DeviceDescription</param>
        /// <returns>200 if successful</returns>
        Task<ServerResponse> SubmitUserTokenRegistrationAsync(string userToken, string pushToken, string deviceUuid, string deviceDescription);

        /// <summary>
        /// Removes the device's registration for push notifications
        /// </summary>
        /// <param name="token"></param>
        /// <param name="uuid"></param>
        /// <returns>200 if successful</returns>
        Task<ServerResponse> SubmitPortalDeregistrationAsync(string token, string uuid);
    }
}

