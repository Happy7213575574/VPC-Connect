using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectApp.AppLog;
using ConnectApp.Entities;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

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
