using System;
using Xamarin.Essentials;

namespace ConnectApp.Pages.Models
{
    public class HomePageModel : AbstractBaseModel
    {
        public HomePageModel() : base()
        {
        }

        private bool showRegistrationSuccess;
        public bool ShowRegistrationSuccess
        {
            get { return showRegistrationSuccess; }
            set { showRegistrationSuccess = value; Notify(nameof(ShowRegistrationSuccess)); }
        }

        private bool showRegistrationInstruction;
        public bool ShowRegistrationInstruction
        {
            get { return showRegistrationInstruction; }
            set { showRegistrationInstruction = value; Notify(nameof(ShowRegistrationInstruction)); }
        }

    }
}
