using System;
using ConnectApp.Maui.Api;
using ConnectApp.Maui.Devices;

namespace ConnectApp.Maui.Pages.Models
{
    public class DebugPageModel : AbstractBaseModel
    {
        App app;

        public DebugPageModel(App app) : base()
        {
            this.app = app;
            DisplayToken = app.LatestPushToken;
        }

        public string DisplayTitle { get { return "VPC Connect " + Version; } }

        public string Build { get { return VersionTracking.CurrentBuild; } }

        public string DeviceDescription { get { return ConnectDevice.DeviceDescription; } }
        public string DeviceUUID { get { return ConnectDevice.UUID; } }

        private string token;
        public string DisplayToken {
            get
            {
#if DEBUG
                return token;
#else
                return string.IsNullOrWhiteSpace(token) ? "(null)" : "present";
#endif
            }
            set
            {
                token = value; Notify(nameof(DisplayToken));
            }
        }

        public string DisplayAppSummary
        {
            get
            {
                return
                    "App version: " + Version + "\n" +
                    "App build: " + Build;
            }
        }

        public string DisplayDeviceSummary
        {
            get
            {
                return
                    "Device: " + DeviceDescription + "\n" +
                    "UUID: " + DeviceUUID;
            }
        }

        public string DisplayEnvironment
        {
            get
            {
#if TESTPORTAL
                var portal = "TEST portal";
#else
                var portal = "LIVE portal";
#endif

#if DEBUG
                var build = "DEBUG build";
#else
                var build = "RELEASE build";
#endif

                return build + "; " + portal;
            }
        }

        public string DisplayPortalApiBaseUri
        {
            get
            {
                return PortalUris.PortalApi_BaseUri;
            }
        }

        public string DisplayPortalSignInUri
        {
            get
            {
                return PortalUris.PortalWeb_LoginUri;
            }
        }
    }
}
