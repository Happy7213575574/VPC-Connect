using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ConnectApp.Pages.Lists;
using Xamarin.Forms;

namespace ConnectApp.Pages.Models
{
    public class ArchivedNotificationsPageViewModel : AbstractBaseModel
    {
        public ArchivedNotificationsPageViewModel() : base()
        {
            notifications = new ObservableCollection<NotificationListItem>();
        }

        private ObservableCollection<NotificationListItem> notifications;
        public ObservableCollection<NotificationListItem> ArchivedNotifications
        {
            get
            {
                return notifications;
            }
            set
            {
                if (notifications != null) { notifications.CollectionChanged -= Notifications_CollectionChanged; }
                notifications = value;
                if (notifications != null)
                {
                    Notify(nameof(ArchivedNotifications), nameof(ShowList), nameof(ShowNoList));
                    notifications.CollectionChanged += Notifications_CollectionChanged;
                }
            }
        }

        private void Notifications_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Notify(nameof(ShowList), nameof(ShowNoList));
        }

        public bool ShowList { get { return ArchivedNotifications != null && ArchivedNotifications.Count > 0; } }

        public bool ShowNoList { get { return !ShowList; } }

        public override bool RecentNotificationsVisible => false;
    }
}
