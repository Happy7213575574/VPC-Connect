using ConnectApp.Maui.AppLog;

namespace ConnectApp.Maui.Pages.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArchivedNotificationViewCell : ViewCell
    {
        private App app;
        private AppLogger log;

        public ArchivedNotificationViewCell()
        {
            InitializeComponent();
            app = App.Instance;
            log = new AppLogger(app.Db).For(this);
        }

    }
}
