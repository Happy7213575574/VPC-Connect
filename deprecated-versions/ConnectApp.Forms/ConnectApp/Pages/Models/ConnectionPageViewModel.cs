using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ConnectApp.Extensions;
using ConnectApp.Pages.Lists;
using ConnectApp.Text;
using Xamarin.Forms;

namespace ConnectApp.Pages.Models
{
    public class ConnectionPageViewModel : AbstractBaseModel
    {
        public const int MAX_PORTAL_RESPONSE_MESSAGE = 1024;
        public const int MAX_USERNAME = 128;
        public const int MAX_PASSWORD = 128;

        public ConnectionPageViewModel() : base()
        {
        }

        public enum States
        {
            PendingToken,
            PortalTokenCheck,
            FormReady,
            PortalGetUserTokenSubmitting,
            PortalGetUserTokenFail,
            PortalGetUserTokenComplete,
            PortalRegisterSubmitting,
            PortalRegisterFail,
            PortalRegisterComplete,
            PortalDeregisterSubmitting,
            PortalDeregisterFail,
            PortalDeregisterComplete
        }

        public string StateStr { get { return Descriptions.ConnectionStates[State]; } }

        private States state = States.PendingToken;
        public States State
        {
            get { return state; }
            set
            {
                state = value;
                Notify(nameof(State));
                Notify(nameof(StateStr));
            }
        }

        public int MaxUsername { get { return MAX_USERNAME; } }
        public int MaxPassword { get { return MAX_PASSWORD; } }

        private string entryUsername;
        public string EntryUsername
        {
            get { return entryUsername; }
            set { entryUsername = value.Truncate(MAX_USERNAME); Notify(nameof(EntryUsername)); }
        }

        private string entryPassword;
        public string EntryPassword
        {
            get { return entryPassword; }
            set { entryPassword = value.Truncate(MAX_PASSWORD); Notify(nameof(EntryPassword)); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                Notify(nameof(IsBusy), nameof(IsInteractive));
            }
        }
        public bool IsInteractive { get { return !IsBusy; } }

        private bool formVisible;
        public bool FormVisible
        {
            get { return formVisible; }
            set { formVisible = value; Notify(nameof(FormVisible)); }
        }

        // TODO: you need to tidy up how this is set / consumed
        private string formPortalResponseMessage;
        public string FormPortalResponseMessage
        {
            get { return formPortalResponseMessage; }
            set
            {
                formPortalResponseMessage = value.Truncate(MAX_PORTAL_RESPONSE_MESSAGE);
                Notify(nameof(FormPortalResponseMessage), nameof(FormPortalResponseVisible));
            }
        }

        public bool FormPortalResponseVisible {
            get { return !string.IsNullOrWhiteSpace(FormPortalResponseMessage); }
        }

        private string completionMessage;
        public string CompletionMessage
        {
            get { return completionMessage; }
            set
            {
                completionMessage = value;
                Notify(nameof(CompletionMessage));
            }
        }

        private bool completionVisible;
        public bool CompletionVisible
        {
            get { return completionVisible; }
            set
            {
                completionVisible = value;
                Notify(nameof(CompletionVisible), nameof(RecentNotificationsVisible));
            }
        }

        public override bool RecentNotificationsVisible
        {
            get { return false; } // these are better displayed on the home page, or notifications page
            /*
            get { return CompletionVisible && RecentNotifications != null && RecentNotifications.Count > 0; }
            */
        }

        public string ReturnHomeButtonText
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        return "    Return Home...    ";
                    default:
                        return "Return Home...";
                }
            }
        }

        public string SignOutButtonText
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        return "    Sign Out    ";
                    default:
                        return "Sign Out";
                }
            }
        }
    }
}
