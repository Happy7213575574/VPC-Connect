using System;
using SQLite;

namespace ConnectApp.Maui.Data.Entities
{
    public class DbState
    {
        public DbState()
        {
        }

        public DbState(int version)
        {
            Recorded = DateTime.Now;
            DbVersion = version;
        }

        [PrimaryKey]
        [NotNull]
        [AutoIncrement]
        public int? DbStateId { get; set; }

        public DateTime Recorded { get; set; }

        public int DbVersion { get; set; }
    }
}

