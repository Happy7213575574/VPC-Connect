using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ConnectApp.Extensions;
using ConnectApp.Pages.Lists;
using ConnectApp.Pages.Models;
using Xamarin.Forms;

namespace ConnectApp.Pages
{
    public partial class ArchivedNotificationsPage : BaseAppContentPage<ArchivedNotificationsPageViewModel>
    {
        public static readonly int MAX_NOTIFICATIONS_ARCHIVED = 1000;

        public ArchivedNotificationsPage(App app = null) : base(app)
        {
            InitializeComponent();
            this.app = app;
        }

        protected override ArchivedNotificationsPageViewModel InitModel(App app)
        {
            return new ArchivedNotificationsPageViewModel();
        }

        #region Lifecycle

        protected override void OnAppearing()
        {
            app.Log.Debug("NotificationsPage.OnAppearing", false);
            base.OnAppearing();

            var notifications = app.Db.GetNotificationRecords(MAX_NOTIFICATIONS_ARCHIVED, archived: true);

            Model.ArchivedNotifications =
                new ObservableCollection<NotificationListItem>(
                    notifications.Select(n => new NotificationListItem(n)));

            app.OnNotificationsErased += App_OnNotificationsErased;
        }

        protected override void OnDisappearing()
        {
            app.Log.Debug("NotificationsPage.OnDisappearing", false);
            base.OnDisappearing();
            app.OnNotificationsErased -= App_OnNotificationsErased;

            Model.ArchivedNotifications.Clear();
        }

        #endregion

        #region Events

        private void App_OnNotificationsErased()
        {
            app.Log.Debug("NotificationsPage.App_OnNotificationsErased", false);
            Device.BeginInvokeOnMainThread(() =>
            {
                Model.ArchivedNotifications.Clear();
            });
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
            app.Log.Debug("ListView item tapped from ArchivedNotificationsPage", false);
            base.ListView_ItemTapped(sender, e).SafeFireAndForget(true);
        }

        #endregion

    }
}
