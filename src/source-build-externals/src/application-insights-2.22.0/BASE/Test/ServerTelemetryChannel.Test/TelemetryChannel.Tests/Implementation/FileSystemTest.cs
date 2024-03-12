namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.IO;
    using System.Linq;
    
    public class FileSystemTest
    {
        protected static string GetUniqueFileName()
        {
            return Guid.NewGuid().ToString("N");
        }

        protected static FileInfo CreatePlatformFile(string fileName, DirectoryInfo folder = null)
        {
            folder = folder ?? GetLocalFolder();
            var file = new FileInfo(Path.Combine(folder.FullName, fileName));
            using (file.Create())
            {
            }

            return file;
        }

        protected static DirectoryInfo GetLocalFolder()
        {
            return new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create));
        }

        protected static DirectoryInfo CreatePlatformFolder(string uniqueName)
        {
            var localFolder = GetLocalFolder();
            return localFolder.CreateSubdirectory(uniqueName);
        }

        protected static void DeletePlatformItem(FileSystemInfo platformItem)
        {
            // If platformItem is a directory, then force delete its subdirectories
            var directory = platformItem as DirectoryInfo;
            if (directory != null)
            {
                directory.Delete(true);
                return;
            }

            platformItem.Delete();
        }
 
        protected static FileInfo GetPlatformFile(string fileName, DirectoryInfo folder = null)
        {
            folder = folder ?? GetLocalFolder();
            var file = folder.GetFiles(fileName).FirstOrDefault();
            if (file == null)
            {
                throw new FileNotFoundException();
            }

            return file;
        }

        protected static DateTimeOffset GetPlatformFileDateCreated(FileInfo platformFile)
        {
            return platformFile.CreationTime;
        }

        protected static string GetPlatformFileName(FileInfo platformFile)
        {
            return platformFile.Name;
        }

        protected static Stream OpenPlatformFile(FileInfo platformFile)
        {
            return platformFile.Open(FileMode.Open);
        }
    }
}