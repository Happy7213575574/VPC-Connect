using System;

namespace ConnectApp.Maui.Devices
{
    public class ConnectDevice
    {
        /// <summary>
        /// A unique id representing this app on this device.
        /// This id will change if the app is reinstalled.
        /// </summary>
        public static string UUID
        {
            get
            {
                var deviceId = Preferences.Get("UniqueDeviceId", string.Empty);
                if (string.IsNullOrWhiteSpace(deviceId))
                {
                    deviceId = System.Guid.NewGuid().ToString();
                    Preferences.Set("UniqueDeviceId", deviceId);
                }
                return deviceId;
            }
        }

        /// <summary>
        /// Always starts with the platform (expected to be: "Android" or "iOS")
        /// </summary>
        public static string DeviceDescription
        {
            get
            {
                return $"{DeviceInfo.Platform.ToString()}: {DeviceInfo.Manufacturer}, {DeviceInfo.Model}, {DeviceInfo.VersionString}";
            }
        }

    }
}
