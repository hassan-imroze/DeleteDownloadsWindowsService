using Syroot.Windows.IO;
using System;
using Topshelf;

namespace DeleteDownloadedFilesService
{
    class Program
    {
        static void Main(string[] args)
        {
            string downloadsPath = new KnownFolder(KnownFolderType.Downloads).Path;
            string archiveFolderPath = "D:\\Downloads Archive";
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<DeleteOlderFiles>(s =>
                {
                    s.ConstructUsing(deleteOlderFiles => new DeleteOlderFiles(downloadsPath,archiveFolderPath));
                    s.WhenStarted(deleteOlderFiles => deleteOlderFiles.Start());
                    s.WhenStopped(deleteOlderFiles => deleteOlderFiles.Stop());
                });

                x.RunAsLocalSystem();

                x.SetServiceName("DeleteOlderDownloadFiles");
                x.SetDisplayName("Delete Download Files");
                x.SetDescription("Delete downloaded files older than 15 days from downloads folder and archive them");

            });


            Environment.ExitCode = (int)exitCode;

        }
    }
}
