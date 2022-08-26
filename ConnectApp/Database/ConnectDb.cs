using System;
using System.Collections.Generic;
using System.Linq;
using ConnectApp.Entities;
using SQLite;

namespace ConnectApp.Database
{
    public class ConnectDb
    {
        private SQLiteConnection Db;

        // always update DbConstants.Version value when altering entities in the db
        private Type[] tableTypes = new[]
        {
            typeof(NotificationRecord),
            typeof(PushConfiguration),
            typeof(LogEntry),
            typeof(DbState)
        };

        public ConnectDb()
        {
            Db = new SQLiteConnection(DbConstants.DatabasePath, DbConstants.Flags);

            // check for table version data
            if (!Db.TableMappings.Any(m => m.MappedType.Name == typeof(DbState).Name) ||
                GetLatestDbState() == null ||
                GetLatestDbState().DbVersion < DbConstants.Version)
            {
                // drop all table mappings
                foreach (var mapping in Db.TableMappings) { Db.DropTable(mapping); }

                // create fresh tables
                Db.CreateTables(CreateFlags.ImplicitIndex, tableTypes);

                // insert fresh db state
                Insert(new DbState(DbConstants.Version));
            }
        }

        public List<NotificationRecord> GetNotificationRecords(bool archived = false)
        {
            return Db.Table<NotificationRecord>()
                .Where(r => r.Archived == archived)
                .OrderByDescending(r => r.Received)
                .ToList();
        }

        public List<NotificationRecord> GetNotificationRecords(int take, int skip = 0, bool archived = false)
        {
            return Db.Table<NotificationRecord>()
                .Where(r => r.Archived == archived)
                .OrderByDescending(r => r.Received)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        public List<LogEntry> GetLogEntries(int take, int skip = 0)
        {
            return Db.Table<LogEntry>()
                .OrderByDescending(r => r.Logged)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        public int Insert(DbState state)
        {
            return Db.InsertOrReplace(state);
        }

        public int Insert(LogEntry log)
        {
            return Db.InsertOrReplace(log);
        }

        public int Insert(NotificationRecord item)
        {
            return Db.InsertOrReplace(item);
        }

        public List<NotificationRecord> GetNotificationsByJson(string json, bool archived = false)
        {
            return Db.Table<NotificationRecord>()
                .Where(r => r.Archived == archived)
                .Where(n => n.AsJson == json)
                .OrderByDescending(r => r.Received)
                .ToList();
        }

        public List<DbState> GetDbStateHistory()
        {
            return Db.Table<DbState>()
                .OrderByDescending(s => s.Recorded)
                .ToList();
        }

        public DbState GetLatestDbState()
        {
            return Db.Table<DbState>()
                .OrderByDescending(s => s.Recorded)
                .FirstOrDefault();
        }

        public PushConfiguration GetPushConfiguration()
        {
            return Db.Table<PushConfiguration>().FirstOrDefault();
        }

        public int SavePushConfiguration(PushConfiguration item)
        {
            return Db.InsertOrReplace(item);
        }

        public int Archive(NotificationRecord record)
        {
            record.Archived = true;
            record.ArchiveDate = DateTime.Now;
            return Db.InsertOrReplace(record);
        }

        private int Delete(NotificationRecord record)
        {
            return Db.Delete(record);
        }

        public int EraseNotificationRecords()
        {
            return Db.DeleteAll<NotificationRecord>();
        }

        public int EraseLogRecords()
        {
            return Db.DeleteAll<LogEntry>();
        }

    }
}
