using System;
using System.Collections.Generic;
using SQLite;

namespace ConnectApp.Entities
{
    public class LogEntry
    {
        public enum LogLevel
        {
            Verbose = 1,
            Debug = 2,
            Info = 3,
            Warning = 4,
            Error = 5,
            Exception = 6
        }

        public LogEntry()
        {
        }

        [PrimaryKey]
        [NotNull]
        [AutoIncrement]
        public int? LogEntryId { get; set; }

        public int DbVersion { get; set; }

        [MaxLength(20)]
        public string AppVersion { get; set; }

        [MaxLength(20)]
        public string AppBuild { get; set; }

        [MaxLength(255)]
        public string DeviceDescription { get; set; }

        [MaxLength(36)]
        public string DeviceUUID { get; set; }

        public DateTime Logged { get; set; }

        public LogLevel Level { get; set; }

        [MaxLength(255)]
        public string Subject { get; set; }

        [MaxLength(4096)]
        public string Message { get; set; }

        [MaxLength(4096)]
        public string StackTrace { get; set; }

        [MaxLength(255)]
        public string ExceptionType { get; set; }
    }
}
