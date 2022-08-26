using System;
using ConnectApp.Extensions;
using ConnectApp.Pages.Models;

namespace ConnectApp.Pages
{
    public partial class HomePage : BaseAppContentPage<HomePageModel>
    {
        public HomePage(App app = null) : base(app)
        {
            InitializeComponent();
        }

        protected override HomePageModel InitModel(App app)
        {
            return new HomePageModel();
        }

        protected override void OnAppActivity(App.AppActivity state)
        {
            // NOP
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            OnAppActivity(app.LastActivity);
            OnRegistrationStateChanged(app.RegistrationState);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        protected override void OnRegistrationStateChanged(App.RegistrationStates state)
        {
            Model.ShowRegistrationSuccess = state == App.RegistrationStates.Registered;
            Model.ShowRegistrationInstruction = state == App.RegistrationStates.NotRegistered;
        }

        void Register_Clicked(object sender, EventArgs e)
        {
            app.Log.Debug("Homepage.Register_Clicked", false);
            app.SwitchToPage(PageTypes.ConnectionPage);
        }

        new void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            app.Log.Debug("ListView item tapped from HomePage", false);
            base.ListView_ItemTapped(sender, e).SafeFireAndForget(true);
        }
    }
}
