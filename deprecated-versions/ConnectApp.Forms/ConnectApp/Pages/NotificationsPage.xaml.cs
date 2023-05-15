using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ConnectApp.Extensions;
using ConnectApp.Pages.Lists;
using ConnectApp.Pages.Models;
using Xamarin.Forms;

namespace ConnectApp.Pages
{
    public partial class NotificationsPage : BaseAppContentPage<NotificationsPageViewModel>
    {
        public static readonly int MAX_NOTIFICATIONS_ALL = 1000;

        public NotificationsPage(App app = null): base(app)
        {
            InitializeComponent();
            this.app = app;
        }

        protected override NotificationsPageViewModel InitModel(App app)
        {
            return new NotificationsPageViewModel();
        }

        #region Lifecycle

        protected override void OnAppearing()
        {
            app.Log.Debug("NotificationsPage.OnAppearing", false);
            base.OnAppearing();

            var notifications = app.Db.GetNotificationRecords(MAX_NOTIFICATIONS_ALL);

            Model.AllNotifications =
                new ObservableCollection<NotificationListItem>(
                    notifications.Select(n => new NotificationListItem(n)));

            app.OnNotificationReceived += App_OnNotificationReceived;
            app.OnNotificationsErased += App_OnNotificationsErased;
            app.OnNotificationDeleted += App_OnNotificationDeleted;
        }

        protected override void OnDisappearing()
        {
            app.Log.Debug("NotificationsPage.OnDisappearing", false);
            base.OnDisappearing();
            app.OnNotificationReceived -= App_OnNotificationReceived;
            app.OnNotificationsErased -= App_OnNotificationsErased;
            app.OnNotificationDeleted -= App_OnNotificationDeleted;

            Model.AllNotifications.Clear();
        }

        #endregion

        #region Events

        private void App_OnNotificationDeleted(Entities.NotificationRecord notification)
        {
            app.Log.Debug("NotificationsPage.App_OnNotificationDeleted", false);
            Device.BeginInvokeOnMainThread(() =>
            {
                var existing = Model.AllNotifications.SingleOrDefault(n => n.Record.NotificationId == notification.NotificationId);

                if (existing != null)
                {
                    Model.AllNotifications.Remove(existing);
                }
            });
        }

        private void App_OnNotificationsErased()
        {
            app.Log.Debug("NotificationsPage.App_OnNotificationsErased", false);
            Device.BeginInvokeOnMainThread(() =>
            {
                Model.AllNotifications.Clear();
            });
        }

        private void App_OnNotificationReceived(Entities.NotificationRecord notification)
        {
            // Unlike RecentNotifications, AllNotifications "doesn't" have a "limit".
            // (Well, the limit is "very high".) We assume it's always safe to insert.
            Model.AllNotifications.Insert(0, new NotificationListItem(notification));
        }

        protected override void OnAppActivity(App.AppActivity state)
        {
            // NOP - not monitoring app state
        }

        protected override void OnRegistrationStateChanged(App.RegistrationStates state)
        {
            // NOP
        }

        new void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            app.Log.Debug("ListView item tapped from NotificationsPage", false);
            base.ListView_ItemTapped(sender, e).SafeFireAndForget(true);
        }

        #endregion

    }
}
