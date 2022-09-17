using System;
using SQLite;

namespace ConnectApp.Entities
{
    public class PushConfiguration
    {
        public const int MAX_PUSH_TOKEN = 255;
        public const int MAX_USER_TOKEN = 255;

        [PrimaryKey]
        [NotNull]
        [AutoIncrement]
        public int? ConfigId { get; set; }

        [MaxLength(MAX_PUSH_TOKEN)]
        public string PushToken { get; set; }

        [MaxLength(MAX_USER_TOKEN)]
        public string UserToken { get; set; }
    }
}
