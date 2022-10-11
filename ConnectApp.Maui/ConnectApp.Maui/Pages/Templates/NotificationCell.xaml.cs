using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectApp.Maui.AppLog;
using ConnectApp.Maui.Data.Entities;

namespace ConnectApp.Maui.Pages.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationCell : ViewCell
    {
        private App app;
        private AppLogger log;

        public NotificationCell()
        {
            InitializeComponent();
            app = App.Instance;
            log = new AppLogger(app.Db).For(this);
        }

        //public void SwipeItem_Clicked(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException("swipeitem clicked");
        //    var item = (SwipeItem)sender;
        //    var record = (NotificationRecord)item.CommandParameter;

        //    if (record != null && record.NotificationId.HasValue)
        //    {
        //        log.Log(LogEntry.LogLevel.Debug, "Requesting deletion of NotificationId: " + record.NotificationId.Value, false);
        //        app.RequestNotificationDeletion(record);
        //    }
        //    else
        //    {
        //        if (record == null)
        //        {
        //            log.Log(LogEntry.LogLevel.Error, "No record found to delete.", false);
        //        }
        //        if (record != null && (record.NotificationId == null || !record.NotificationId.HasValue))
        //        {
        //            log.Log(LogEntry.LogLevel.Error, "Record for deletion does not have a NotificationId.", false);
        //        }
        //    }
        //}
    }
}
