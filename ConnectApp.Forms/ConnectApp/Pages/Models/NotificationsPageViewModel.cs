using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ConnectApp.Pages.Lists;

namespace ConnectApp.Pages.Models
{
    public class NotificationsPageViewModel : AbstractBaseModel
    {
        public NotificationsPageViewModel() : base()
        {
            notifications = new ObservableCollection<NotificationListItem>();
        }

        private ObservableCollection<NotificationListItem> notifications;
        public ObservableCollection<NotificationListItem> AllNotifications
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
                    Notify(nameof(AllNotifications), nameof(ShowList), nameof(ShowNoList));
                    notifications.CollectionChanged += Notifications_CollectionChanged;
                }
            }
        }

        private void Notifications_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Notify(nameof(ShowList), nameof(ShowNoList));
        }

        public bool ShowList { get { return AllNotifications != null && AllNotifications.Count > 0; } }

        public bool ShowNoList { get { return !ShowList; } }

        public override bool RecentNotificationsVisible => false;
    }
}
