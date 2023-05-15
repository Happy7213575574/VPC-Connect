using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectApp.AppLog;
using ConnectApp.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectApp.Pages.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArchivedNotificationViewCell : ViewCell
    {
        private App app;
        private AppLogger log;

        public ArchivedNotificationViewCell()
        {
            InitializeComponent();
            app = ConnectApp.App.Instance;
            log = new AppLogger(app.Db).For(this);
        }

    }
}
