using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConnectApp.Maui.Data;
using CsvHelper;

namespace ConnectApp.Maui.AppLog
{
    public class AppLogExporter
    {
        private ConnectAppData db;

        public AppLogExporter(ConnectAppData db)
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
