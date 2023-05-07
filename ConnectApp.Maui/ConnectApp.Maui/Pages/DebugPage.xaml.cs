using ConnectApp.Maui.AppLog;
using ConnectApp.Maui.Pages.Models;

namespace ConnectApp.Maui.Pages
{
    public partial class DebugPage : BaseAppContentPage<DebugPageModel>
    {
        public DebugPage() : base()
        {
            InitializeComponent();
        }

        protected override DebugPageModel InitModel(App app)
        {
            return new DebugPageModel(app);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Model.DisplayToken = app.LatestPushToken;
            app.OnPushTokenChange += App_OnPushTokenChange;

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            app.OnPushTokenChange -= App_OnPushTokenChange;
        }

        private void App_OnPushTokenChange(string token)
        {
            Model.DisplayToken = token;
        }

        protected override void OnRegistrationStateChanged(App.RegistrationStates state)
        {
            // NOP
        }

        protected override void OnAppActivity(App.AppActivity state)
        {
            // NOP
        }

        protected async void Button_ExportClicked(object sender, EventArgs e)
        {
            var exporter = new AppLogExporter(app.Db);
            await exporter.ExportAsync();
        }

        protected async void Button_TestNotifiationClicked(object sender, EventArgs e)
        {
            var dictionary = new Dictionary<string, string>();

            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            
            var chars = string.Join("",
                Enumerable
                    .Repeat(alphabet, 6)
                    .Select(s => s[App.Random.Next(s.Length)])
                    .ToArray());

            dictionary["title"] = "TEST notification " + chars;
            dictionary["body"] = "This is a quick test: " + chars;
            dictionary["sectionURL"] = "https://www.google.com";

            var record = app.RequestAddNotification(dictionary);
            await DisplayAlert("Notification " + record.NotificationId, record.AsJson, "Ok");
        }

        void Button_SimulateCrashClicked(object sender, EventArgs e)
        {
            app.Crashlytics.SimulateCrash();
        }
    }
}
