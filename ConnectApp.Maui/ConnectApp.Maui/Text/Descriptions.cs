using System;
using System.Collections.Generic;
using static ConnectApp.Maui.App;
using static ConnectApp.Maui.Pages.Models.ConnectionPageViewModel;

namespace ConnectApp.Maui.Text
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
                    return new Tuple<string, Color>("Initialising...", Colors.Blue);

                case App.AppEvents.PushTokenCheckInitiated:
                    return new Tuple<string, Color>("Checking registration...", Colors.Blue);

                case App.AppEvents.PushTokenCheckResult:
                    return state.Result.IsSuccess ?
                        new Tuple<string, Color>("Registration confirmed.", Colors.Green) :
                        new Tuple<string, Color>("Please register.", Colors.Red);

                case AppEvents.GetUserTokenInitiated:
                    return state.Result.IsSuccess ?
                        new Tuple<string, Color>("User token retrieved.", Colors.Green) :
                        new Tuple<string, Color>("User token unavailable.", Colors.Red);

                case AppEvents.GetUserTokenResult:
                    return new Tuple<string, Color>("Obtaining user token...", Colors.Blue);

                case App.AppEvents.SubmitRegistrationInitiated:
                    return new Tuple<string, Color>("Submitting registration...", Colors.Blue);

                case App.AppEvents.SubmitRegistrationResult:
                    return state.Result.IsSuccess ?
                        new Tuple<string, Color>("Registration succeeded.", Colors.Green) :
                        new Tuple<string, Color>("Registration unsuccessful.", Colors.Red);

                case App.AppEvents.SubmitDeregistrationInitiated:
                    return new Tuple<string, Color>("Signing out...", Colors.Blue);

                case App.AppEvents.SubmitDeregistrationResult:
                    return state.Result.IsSuccess ?
                        new Tuple<string, Color>("Signed out.", Colors.Orange) :
                        new Tuple<string, Color>("An error occurred during sign out.", Colors.Red);

                default:
                    throw new NotImplementedException("App state description not implemented: " + state.State.ToString());
            }
        }

    }
}
