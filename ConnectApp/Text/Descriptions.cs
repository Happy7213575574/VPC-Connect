using System;
using System.Collections.Generic;
using Xamarin.Forms;
using static ConnectApp.App;
using static ConnectApp.Pages.Models.ConnectionPageViewModel;

namespace ConnectApp.Text
{
    public static class Descriptions
    {
        public static Dictionary<States, string> ConnectionStates = new Dictionary<States, string>()
        {
            { States.FormReady, "Ready." },
            { States.PendingToken, "Initialising token..." },
            { States.PortalRegisterComplete, "Registration complete." },
            { States.PortalRegisterFail, "Registration error." },
            { States.PortalGetUserTokenSubmitting, "Obtaining user token..." },
            { States.PortalGetUserTokenComplete, "Obtained user token." },
            { States.PortalGetUserTokenFail, "Unable to obtain user token." },
            { States.PortalRegisterSubmitting, "Submitting registration..." },
            { States.PortalTokenCheck, "Checking token..." },
            { States.PortalDeregisterComplete, "Signed out." },
            { States.PortalDeregisterFail, "Unable to deregister." },
            { States.PortalDeregisterSubmitting, "Submitting sign out..." }
        };

        public static Tuple<string, Color> DescribeAppState(AppActivity state)
        {
            switch (state.State)
            {
                case App.AppEvents.Initialising:
                    return new Tuple<string, Color>("Initialising...", Color.Blue);

                case App.AppEvents.PushTokenCheckInitiated:
                    return new Tuple<string, Color>("Checking registration...", Color.Blue);

                case App.AppEvents.PushTokenCheckResult:
                    return state.Result.IsSuccess ?
                        new Tuple<string, Color>("Registration confirmed.", Color.Green) :
                        new Tuple<string, Color>("Please register.", Color.Red);

                case AppEvents.GetUserTokenInitiated:
                    return state.Result.IsSuccess ?
                        new Tuple<string, Color>("User token retrieved.", Color.Green) :
                        new Tuple<string, Color>("User token unavailable.", Color.Red);

                case AppEvents.GetUserTokenResult:
                    return new Tuple<string, Color>("Obtaining user token...", Color.Blue);

                case App.AppEvents.SubmitRegistrationInitiated:
                    return new Tuple<string, Color>("Submitting registration...", Color.Blue);

                case App.AppEvents.SubmitRegistrationResult:
                    return state.Result.IsSuccess ?
                        new Tuple<string, Color>("Registration succeeded.", Color.Green) :
                        new Tuple<string, Color>("Registration unsuccessful.", Color.Red);

                case App.AppEvents.SubmitDeregistrationInitiated:
                    return new Tuple<string, Color>("Signing out...", Color.Blue);

                case App.AppEvents.SubmitDeregistrationResult:
                    return state.Result.IsSuccess ?
                        new Tuple<string, Color>("Signed out.", Color.Orange) :
                        new Tuple<string, Color>("An error occurred during sign out.", Color.Red);

                default:
                    throw new NotImplementedException("App state description not implemented: " + state.State.ToString());
            }
        }

    }
}
