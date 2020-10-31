using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DeleteDownloadedFilesService
{
    public class DeleteOlderFiles
    {
        private readonly Timer _timer;
        private string _deleteFolderPath;
        private string _archiveFolderPath;
        public DeleteOlderFiles(string deleteFolderPath,string archiveFolderPath)
        {
            _deleteFolderPath = deleteFolderPath;
            _archiveFolderPath = archiveFolderPath;
            var eightHours = (1000 * 60) * 60 * 8;
            _timer = new Timer(eightHours) { AutoReset = true };
            _timer.Elapsed += DeleteOldFiles;
        }
        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }
        public void DeleteOldFiles(object sender, ElapsedEventArgs e)
        {
            var files = Directory.GetFiles(_deleteFolderPath);

            foreach (var file in files)
            {
                var modifiedTimeUtc = File.GetLastWriteTimeUtc(file);
                if((DateTime.UtcNow - modifiedTimeUtc).TotalDays >= 15)
                {
                    File.Copy(file, GetNewFilePath(file, modifiedTimeUtc)); 
                    File.Delete(file);
                }
            }

        }

        private string GetNewFilePath(string file, DateTime modifiedDate)
        {
            string fileName = file.Split('\\').Reverse().FirstOrDefault();
            string newFolderPath = $"{_archiveFolderPath}\\{modifiedDate:yyyy-MMM}";
            if(!Directory.Exists(newFolderPath))
            {
                Directory.CreateDirectory(newFolderPath);
            }
            return $"{newFolderPath}\\{fileName}";
        }
    }
}
