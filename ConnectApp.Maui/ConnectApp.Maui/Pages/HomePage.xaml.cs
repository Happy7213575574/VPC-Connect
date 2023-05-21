using ConnectApp.Maui.Extensions;
using ConnectApp.Maui.Pages.Models;

namespace ConnectApp.Maui.Pages
{
    public partial class HomePage : BaseAppContentPage<HomePageModel>
    {
        public HomePage() : base()
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

        public async void Register_Clicked(object sender, EventArgs e)
        {
            app.Log.Debug("Homepage.Register_Clicked", false);
            await app.SwitchToPageAsync(PageTypes.ConnectionPage);
        }

        new void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            app.Log.Debug("ListView item tapped from HomePage", false);
            base.ListView_ItemTapped(sender, e).SafeFireAndForget(true);
        }

        new async void SeeAll_Tapped(object sender, EventArgs e)
        {
            app.Log.Debug("SeeAll_Tapped from HomePage", false);
            await base.SeeAll_Tapped(sender, e);
        }
    }
}
