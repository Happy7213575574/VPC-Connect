using System;
namespace ConnectApp.Maui.Data
{
    public static class DbConstants
    {
        public const int Version = 4;

        public const string DatabaseFilename = "vpc_connect.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ProtectionCompleteUnlessOpen |
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = FileSystem.Current.AppDataDirectory;
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}

