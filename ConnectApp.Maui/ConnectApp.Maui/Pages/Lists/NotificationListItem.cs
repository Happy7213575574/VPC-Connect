using System;
using System.ComponentModel;
using System.Windows.Input;
using ConnectApp.Maui.Api;
using ConnectApp.Maui.Data.Entities;
using ConnectApp.Maui.Extensions;
using ConnectApp.Maui.Pages.Models;

namespace ConnectApp.Maui.Pages.Lists
{
    public class NotificationListItem : INotifyPropertyChanged
    {
        private App app;

        public NotificationListItem(NotificationRecord record, ICommand tapCommand)
        {
            app = App.Instance;

            Record = record;
            ArchiveNotificationCommand = new Command(OnArchiveNotificationRequested);
            TapCommand = tapCommand;
        }

        private Uri CalculateUri(NotificationRecord record)
        {
            return new Uri(record.TargetUrl ?? PortalUris.PortalWeb_LoginUri);
        }

        private string text;
        public string DisplayText
        {
            get { return text; }
            private set
            {
                text = value.Truncate(NotificationRecord.MAX_TITLE);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayText)));
            }
        }

        private Uri uri;
        public Uri Uri
        {
            get { return uri; }
            private set
            {
                uri = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Uri)));
            }
        }

        private string content;
        public string DisplayContent
        {
            get { return content; }
            private set
            {
                content = value.Truncate(NotificationRecord.MAX_MESSAGE);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayContent)));
            }
        }

        private string date;
        public string DisplayDate
        {
            get { return date; }
            private set
            {
                date = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayDate)));
            }
        }

        private string messageCount;
        public string DisplayMessageCount
        {
            get { return messageCount; }
            private set
            {
                messageCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayMessageCount)));
            }
        }

        private NotificationRecord record;
        public NotificationRecord Record {
            get { return record; }
            set
            {
                record = value;
                DisplayText = record.Title;
                DisplayContent = record.Message;
                DisplayDate = $"{record.Received.ToShortDateString()}\n{record.Received.ToShortTimeString()}";
                DisplayMessageCount = record.ReceiveCount.ToString();
                Uri = CalculateUri(record);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand TapCommand { get; private set; }

        public ICommand ArchiveNotificationCommand { get; private set; }

        void OnArchiveNotificationRequested(object parameter)
        {
            var record = (NotificationRecord)parameter;

            if (record != null && record.NotificationId.HasValue)
            {
                app.Log.Debug("Requesting deletion of NotificationId: " + record.NotificationId.Value, false);
                app.RequestNotificationDeletion(record);
            }
            else
            {
                if (record == null)
                {
                    app.Log.Error("No record found to delete.", false);
                }
                if (record != null && (record.NotificationId == null || !record.NotificationId.HasValue))
                {
                    app.Log.Error("Record for deletion does not have a NotificationId.", false);
                }
            }
        }

    }
}
