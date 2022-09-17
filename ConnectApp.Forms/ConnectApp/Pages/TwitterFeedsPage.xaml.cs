using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TwitterFeedsPage : TabbedPage
    {
        private App app;

        public TwitterFeedsPage(App app = null)
        {
            InitializeComponent();
            this.app = app;
        }

        public async void WebView_NavigatingAsync(object sender, WebNavigatingEventArgs args)
        {
            // local files
            if (args.Url.StartsWith("file://")) { return; }

            // init
            if (args.Url.ToLower().Contains("about:blank")) { return; }

            // standard twitter API calls
            if (args.Url.ToLower().Contains("syndication.twitter.com")) { return; }
            if (args.Url.ToLower().Contains("platform.twitter.com")) { return; }

            // only visit twitter, always cancel the embedded navigation
            if (args.NavigationEvent == WebNavigationEvent.NewPage &&
                args.Url.ToLower().Contains("twitter.com"))
            {
                args.Cancel = true;
                await Launcher.OpenAsync(new Uri(args.Url));
            }

            args.Cancel = true;
        }
    }
}
