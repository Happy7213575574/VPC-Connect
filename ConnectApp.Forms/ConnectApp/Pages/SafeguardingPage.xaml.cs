using System;
using System.Collections.Generic;
using ConnectApp.Pages.Models;
using Xamarin.Forms;

namespace ConnectApp.Pages
{
    public partial class SafeguardingPage : BaseAppContentPage<SafeguardingPageModel>
    {
        public SafeguardingPage(App app = null) : base(app)
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
