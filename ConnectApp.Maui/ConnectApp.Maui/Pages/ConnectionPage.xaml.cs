using ConnectApp.Maui.Api;
using ConnectApp.Maui.Extensions;
using ConnectApp.Maui.Pages.Models;
using ConnectApp.Maui.Text;
using static ConnectApp.Maui.Pages.Models.ConnectionPageViewModel;

namespace ConnectApp.Maui.Pages
{
    public partial class ConnectionPage : BaseAppContentPage<ConnectionPageViewModel>
    {
        public ConnectionPage() : base()
        {
            log.Debug("ConnectionPage creation.", false);
            InitializeComponent();
        }

        protected override ConnectionPageViewModel InitModel(App app)
        {
            return new ConnectionPageViewModel();
        }

        #region Lifecycle

        protected override void OnAppearing()
        {
            log.Debug("ConnectionPage.OnAppearing", false);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            log.Debug("ConnectionPage.OnDisappearing", false);
            base.OnDisappearing();
        }

        #endregion

        #region state

        private void SetState(States state)
        {
            log.Info("ConnectionPage.SetState: " + state.ToString(), false);
            Model.State = state;
            switch (state)
            {
                case States.PendingToken:
                    Model.IsBusy = true;
                    Model.FormVisible = false;
                    Model.CompletionVisible = false;
                    break;

                case States.PortalTokenCheck:
                    Model.IsBusy = true;
                    Model.FormVisible = false;
                    Model.CompletionVisible = false;
                    break;

                case States.FormReady:
                    Model.FormVisible = true;
                    Model.IsBusy = false;
                    Model.CompletionVisible = false;
                    Model.EntryUsername = "";
                    Model.EntryPassword = "";
                    break;

                case States.PortalGetUserTokenSubmitting:
                    Model.IsBusy = true;
                    Model.FormVisible = true;
                    Model.CompletionVisible = false;
                    break;

                case States.PortalGetUserTokenComplete:
                    Model.IsBusy = false;
                    Model.FormVisible = false;
                    Model.CompletionVisible = true;
                    Model.CompletionMessage = "Registered for notifications.";
                    break;

                case States.PortalGetUserTokenFail:
                    Model.IsBusy = false;
                    Model.FormVisible = true;
                    Model.CompletionVisible = false;
                    break;

                case States.PortalRegisterSubmitting:
                    Model.IsBusy = true;
                    Model.FormVisible = true;
                    Model.CompletionVisible = false;
                    break;

                case States.PortalRegisterFail:
                    Model.IsBusy = false;
                    Model.FormVisible = true;
                    Model.CompletionVisible = false;
                    break;

                case States.PortalRegisterComplete:
                    Model.IsBusy = false;
                    Model.FormVisible = false;
                    Model.CompletionVisible = true;
                    Model.CompletionMessage = "Registered for notifications.";
                    break;

                case States.PortalDeregisterSubmitting:
                    Model.IsBusy = true;
                    Model.FormVisible = false;
                    Model.CompletionVisible = true;
                    break;

                case States.PortalDeregisterFail:
                    Model.IsBusy = false;
                    Model.FormVisible = true;
                    Model.CompletionVisible = false;
                    break;

                case States.PortalDeregisterComplete:
                    Model.IsBusy = false;
                    Model.FormVisible = true;
                    Model.CompletionVisible = false;
                    break;
            }
        }

        private List<string> ValidateForm()
        {
            var validations = new List<string>();
            Model.EntryUsername = Model.EntryUsername?.Trim();
            Model.EntryPassword = Model.EntryPassword?.Trim();
            if (string.IsNullOrWhiteSpace(Model.EntryUsername) ||
                string.IsNullOrWhiteSpace(Model.EntryPassword))
            {
                validations.Add(Validations.FormInvalid);
            }
            return validations;
        }

        #endregion

        #region Events

        protected override void OnAppActivity(App.AppActivity state)
        {
            log.Debug("ConnectionPage.App_OnStateChange: " + state.State.ToString(), false);
            switch (state.State)
            {
                case App.AppEvents.Initialising:
                    SetState(States.PendingToken);
                    break;

                case App.AppEvents.PushTokenCheckInitiated:
                    SetState(States.PortalTokenCheck);
                    break;

                case App.AppEvents.PushTokenCheckResult:
                    SetState(state.Result.IsSuccess ? States.PortalRegisterComplete : States.FormReady);
                    break;

                case App.AppEvents.GetUserTokenInitiated:
                    SetState(States.PortalGetUserTokenSubmitting);
                    break;

                case App.AppEvents.GetUserTokenResult:
                    if (state.Result.IsSuccess)
                    {
                        SetState(States.PortalGetUserTokenComplete);
                    }
                    else
                    {
                        Model.FormPortalResponseMessage = state.Result.StatusDescription;
                        SetState(States.PortalGetUserTokenFail);
                    }
                    break;

                case App.AppEvents.SubmitRegistrationInitiated:
                    SetState(States.PortalRegisterSubmitting);
                    break;

                case App.AppEvents.SubmitRegistrationResult:
                    if (state.Result.IsSuccess)
                    {
                        SetState(States.PortalRegisterComplete);
                    }
                    else
                    {
                        Model.FormPortalResponseMessage = state.Result.StatusDescription;
                        SetState(States.PortalRegisterFail);
                    }
                    break;

                case App.AppEvents.SubmitDeregistrationInitiated:
                    SetState(States.PortalDeregisterSubmitting);
                    break;

                case App.AppEvents.SubmitDeregistrationResult:
                    if (state.Result.IsSuccess)
                    {
                        Model.FormPortalResponseMessage = "Signed out.";
                        Model.EntryPassword = null;
                        SetState(States.PortalDeregisterComplete);
                    }
                    else
                    {
                        Model.FormPortalResponseMessage = state.Result.StatusDescription;
                        SetState(States.PortalDeregisterFail);
                    }
                    break;
            }
        }

        private async void Btn_ReturnHomeClicked(object sender, EventArgs args)
        {
            log.Debug("ConnectionPage.Btn_ReturnHomeClicked", false);
            await app.SwitchToPageAsync(PageTypes.HomePage);
        }

        private async void Btn_ConnectionClicked(object sender, EventArgs args)
        {
            log.Debug("ConnectionPage.Btn_ConnectionClicked", false);
            var validations = ValidateForm();
            if (validations.Count > 0)
            {
                Model.FormPortalResponseMessage = string.Join(" ", validations);
                log.Debug("Form invalid: " + Model.FormPortalResponseMessage, false);
                SetState(States.PortalRegisterFail);
            }
            else
            {
                log.Debug("Form valid.", false);
                await app.GetUserTokenAndRegisterAsync(Model.EntryUsername, Model.EntryPassword, true);
            }
        }

        private async void Btn_SignOutClicked(object sender, EventArgs args)
        {
            log.Debug("ConnectionPage.Btn_SignOutClicked", false);
            var ok = await DisplayAlert(
                "Are you sure?",
                "If you sign out, you will no longer receive notifications from the VPC portal. Previously received portal notifications will be deleted from your phone.",
                "Sign out",
                "Cancel");
            if (ok)
            {
                await app.DeregisterAsync();
            }
        }

        protected override void OnRegistrationStateChanged(App.RegistrationStates state)
        {
            // NOP
        }

        new void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            app.Log.Debug("ListView item tapped from ConnectionPage", false);
            base.ListView_ItemTapped(sender, e).SafeFireAndForget(true);
        }

        #endregion
    }
}
