using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConnectApp.Pages.Lists;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMasterPage : ContentPage
    {
        public ListView ListView;

        public MainMasterPage()
        {
            InitializeComponent();

            BindingContext = new MainMasterDetailMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainMasterDetailMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<PageTypeMenuItem> MenuItems { get; set; }

            public MainMasterDetailMasterViewModel()
            {
                MenuItems = new ObservableCollection<PageTypeMenuItem>(new[]
                {
                    new PageTypeMenuItem(PageTypes.HomePage, "Home"),
                    new PageTypeMenuItem(PageTypes.ConnectionPage, "Connection"),
                    new PageTypeMenuItem(PageTypes.NotificationsPage, "Notifications"),
                    new PageTypeMenuItem(PageTypes.TwitterFeedsPage, "News feeds"),
                    new PageTypeMenuItem(PageTypes.SafeguardingPage, "Safeguarding"),
                    new PageTypeMenuItem(PageTypes.ArchivedNotificationsPage, "Archive"),
#if DEBUG
                    new PageTypeMenuItem(PageTypes.DebugPage, "App info")
#endif
                });
            }

            public string DisplayVersion { get { return "Version " + Version; } }
            public string Version { get { return VersionTracking.CurrentVersion; } }

            public string DisplayEnvironment
            {
                get
                {
#if TESTPORTAL
                var portal = "TEST";
#else
                    var portal = "MVP";
#endif

#if DEBUG
                var build = "debug";
#else
                    var build = "live";
#endif

                    return string.Join(" ", new[] { portal, build });
                }
            }


            public event PropertyChangedEventHandler PropertyChanged;

            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
