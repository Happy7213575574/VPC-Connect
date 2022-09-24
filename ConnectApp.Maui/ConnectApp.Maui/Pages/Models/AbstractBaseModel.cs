using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ConnectApp.Maui.Api;
using ConnectApp.Maui.Pages.Lists;
using ConnectApp.Maui.Text;
using static ConnectApp.Maui.App;

namespace ConnectApp.Maui.Pages.Models
{
    public abstract class AbstractBaseModel : INotifyPropertyChanged
    {
        protected AbstractBaseModel()
        {
            recentNotifications = new ObservableCollection<NotificationListItem>();
            TapLinkCommand = new Command(OnLinkTapped);
        }

        public bool ShowTestPortalIndicator
        {
            get
            {
#if TESTPORTAL
                return true;
#else
                return false;
#endif
            }
        }

        public ICommand TapLinkCommand { get; private set; }

        void OnLinkTapped(object parameter)
        {
            string uri = (string)parameter;
            OnUriRequested?.Invoke(uri);
        }

        private AppActivity appState;
        public AppActivity AppState
        {
            get { return appState; }
            set {
                appState = value;
                Notify(nameof(AppState), nameof(AppStateStr), nameof(AppStateClr));
            }
        }

        public string AppStateStr
        {
            get { return Descriptions.DescribeAppState(AppState).Item1; }
        }

        public Color AppStateClr
        {
            get { return Descriptions.DescribeAppState(AppState).Item2; }
        }

        private string debugLabel;
        public string DebugLabel
        {
            get { return debugLabel; }
            set { debugLabel = value; Notify(nameof(DebugLabel)); }
        }

        private bool debugVisible;
        public bool DebugVisible
        {
            get { return debugVisible; }
            set { debugVisible = value; Notify(nameof(DebugVisible)); }
        }

        private ObservableCollection<NotificationListItem> recentNotifications;
        public ObservableCollection<NotificationListItem> RecentNotifications
        {
            get { return recentNotifications; }
            set
            {
                if (value != recentNotifications)
                {
                    recentNotifications = value;
                    Notify(nameof(RecentNotifications), nameof(RecentNotificationsVisible));
                    recentNotifications.CollectionChanged += RecentNotifications_CollectionChanged;
                }
            }
        }

        private void RecentNotifications_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify(nameof(RecentNotifications), nameof(RecentNotificationsVisible));
        }

        public virtual bool RecentNotificationsVisible
        {
            get { return RecentNotifications != null && RecentNotifications.Count > 0; }
        }

        public virtual string PortalSummaryUri { get { return PortalUris.PortalWeb_BaseUri; } }
        public virtual string PortalDiaryUri { get { return PortalUris.PortalWeb_DiaryUri; } }
        public virtual string PortalCalendarUri { get { return PortalUris.PortalWeb_CalendarUri; } }
        public virtual string PortalConversationsUri { get { return PortalUris.PortalWeb_ConversationsUri; } }
        public virtual string PortalMessageBoardsUri { get { return PortalUris.PortalWeb_MessageBoardsUri; } }
        public virtual string PortalResourcesUri { get { return PortalUris.PortalWeb_ResourcesUri; } }

        public string DisplayVersion { get { return "Version " + Version; } }
        public string Version { get { return VersionTracking.CurrentVersion; } }

        protected void Notify(params string[] names)
        {
            foreach (var name in names)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event Action<string> OnUriRequested;
    }
}
