using System;
using SQLite;

namespace ConnectApp.Maui.Data.Entities
{
    public class NotificationRecord
    {
        public const int MAX_TITLE = 1024;
        public const int MAX_MESSAGE = 2048;
        public const int MAX_JSON = 4096;
        public const int MAX_URL = 2048;

        [PrimaryKey]
        [NotNull]
        [AutoIncrement]
        public int? NotificationId { get; set; }

        public DateTime Received { get; set; }

        public int ReceiveCount { get; set; }

        [MaxLength(MAX_TITLE)]
        public string Title { get; set; }

        [MaxLength(MAX_MESSAGE)]
        public string Message { get; set; }

        [MaxLength(MAX_JSON)]
        public string AsJson { get; set; }

        [MaxLength(MAX_URL)]
        public string TargetUrl { get; set; }

        public bool Archived { get; set; }

        public DateTime ArchiveDate { get; set; }
    }
}