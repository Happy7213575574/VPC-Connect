using System.Collections.ObjectModel;
using ConnectApp.Maui.Extensions;
using ConnectApp.Maui.Pages.Lists;
using ConnectApp.Maui.Pages.Models;

namespace ConnectApp.Maui.Pages
{
    public partial class NotificationsPage : BaseAppContentPage<NotificationsPageViewModel>
    {
        public static readonly int MAX_NOTIFICATIONS_ALL = 1000;

        public NotificationsPage(): base()
        {
            InitializeComponent();
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
                    notifications.Select(n => new NotificationListItem(n, Model.TapLinkCommand)));

            app.OnNotificationReceived += App_OnNotificationReceived;
            app.OnNotificationsErased += App_OnNotificationsErased;
            app.OnNotificationArchived += App_OnNotificationDeleted;
        }

        protected override void OnDisappearing()
        {
            app.Log.Debug("NotificationsPage.OnDisappearing", false);
            base.OnDisappearing();
            app.OnNotificationReceived -= App_OnNotificationReceived;
            app.OnNotificationsErased -= App_OnNotificationsErased;
            app.OnNotificationArchived -= App_OnNotificationDeleted;

            Model.AllNotifications.Clear();
        }

        #endregion

        #region Events

        private void App_OnNotificationDeleted(Data.Entities.NotificationRecord notification)
        {
            app.Log.Debug("NotificationsPage.App_OnNotificationDeleted", false);
            Dispatcher.Dispatch(() =>
            {
                var existing = Model.AllNotifications.SingleOrDefault(n => n.Record.NotificationId == notification.NotificationId);

                if (existing != null)
                {
                    try
                    {
                        Model.AllNotifications.Remove(existing);
                    }
                    catch (Exception e)
                    {
                        // weird exception thrown when removing the last item
                        log.Exception(e);
                    }
                }
            });
        }

        private void App_OnNotificationsErased()
        {
            app.Log.Debug("NotificationsPage.App_OnNotificationsErased", false);
            Dispatcher.Dispatch(() =>
            {
                Model.AllNotifications.Clear();
            });
        }

        private void App_OnNotificationReceived(Data.Entities.NotificationRecord notification)
        {
            // Unlike RecentNotifications, AllNotifications "doesn't" have a "limit".
            // (Well, the limit is "very high".) We assume it's always safe to insert.
            Model.AllNotifications.Insert(0, new NotificationListItem(notification, Model.TapLinkCommand));
        }

        protected override void OnAppActivity(App.AppActivity state)
        {
            // NOP - not monitoring app state
        }

        protected override void OnRegistrationStateChanged(App.RegistrationStates state)
        {
            // NOP
        }

        new void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            app.Log.Debug("ListView item tapped from NotificationsPage", false);
            base.ListView_ItemTapped(sender, e).SafeFireAndForget(true);
        }

        #endregion

    }
}
