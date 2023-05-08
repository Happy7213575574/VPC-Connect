using ConnectApp.Maui.Pages.Models;

namespace ConnectApp.Maui.Pages
{
    public partial class SafeguardingPage : BaseAppContentPage<SafeguardingPageModel>
    {
        public SafeguardingPage() : base()
        {
            InitializeComponent();
        }

        protected override SafeguardingPageModel InitModel(App app)
        {
            return new SafeguardingPageModel();
        }

        protected override void OnAppActivity(App.AppActivity state)
        {
            // not tracking app state
        }

        protected override void OnRegistrationStateChanged(App.RegistrationStates state)
        {
            // not tracking registration state
        }
    }
}
