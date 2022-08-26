using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConnectApp.Database;
using CsvHelper;
using Xamarin.Essentials;

namespace ConnectApp.AppLog
{
    public class AppLogExporter
    {
        private ConnectDb db;

        public AppLogExporter(ConnectDb db)
        {
            this.db = db;
        }

        public async Task ExportAsync()
        {
            var filename = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_log.csv";
            var file = Path.Combine(FileSystem.CacheDirectory, filename);

            var logs = db.GetLogEntries(1000);

            using (var writer = new StreamWriter(file))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(logs);
                }
            }

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Export issue log",
                File = new ShareFile(file)
            });
        }
    }
}
