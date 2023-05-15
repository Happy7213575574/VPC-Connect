using ConnectApp.Pages.Lists;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        private App app;

        public MainPage(App app = null)
        {
            InitializeComponent();
            this.app = app;

            MasterPage.ListView.ItemSelected += ListView_ItemSelected;

            SwitchToPage(PageTypes.HomePage); // start on the home page
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) { return; }
            var item = (PageTypeMenuItem)e.SelectedItem;
            SwitchToPage(item.PageType);
        }

        internal void SwitchToPage(PageTypes page)
        {
            app.Analytics?.SendEvent("PageView", "PageType", page.ToString());

            switch (page)
            {
                case PageTypes.HomePage:
                    Detail = new NavigationPage(new HomePage(app));
                    break;
                case PageTypes.TwitterFeedsPage:
                    Detail = new NavigationPage(new TwitterFeedsPage(app));
                    break;
                case PageTypes.ConnectionPage:
                    Detail = new NavigationPage(new ConnectionPage(app));
                    break;
                case PageTypes.NotificationsPage:
                    Detail = new NavigationPage(new NotificationsPage(app));
                    break;
                case PageTypes.SafeguardingPage:
                    Detail = new NavigationPage(new SafeguardingPage(app));
                    break;
                case PageTypes.DebugPage:
                    Detail = new NavigationPage(new DebugPage(app));
                    break;
                case PageTypes.ArchivedNotificationsPage:
                    Detail = new NavigationPage(new ArchivedNotificationsPage(app));
                    break;
                default:
                    throw new InvalidNavigationException("Page not yet supported: " + page.ToString());
            }

            IsPresented = false;
            MasterPage.ListView.SelectedItem = null;
        }
    }
}
