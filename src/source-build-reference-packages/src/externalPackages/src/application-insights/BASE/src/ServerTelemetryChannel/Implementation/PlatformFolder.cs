namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class PlatformFolder : IPlatformFolder
    {
        private readonly DirectoryInfo folder;

        public PlatformFolder(DirectoryInfo folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            this.folder = folder;
        }

        public string Name
        {
            get { return this.folder.Name; }
        }

        internal DirectoryInfo Folder
        {
            get { return this.folder; }
        }

        public bool Exists()
        {
            return Directory.Exists(this.folder.FullName);
        }

        public void Delete()
        {
            this.folder.Delete();
        }

        public IEnumerable<IPlatformFile> GetFiles()
        {
            try
            {
                return this.folder.GetFiles().Select(fileInfo => new PlatformFile(fileInfo));
            }
            catch (DirectoryNotFoundException)
            {
                // Return empty list for compatibility with Windows runtime.
                return Enumerable.Empty<IPlatformFile>();
            }
        }

        public IPlatformFile CreateFile(string fileName)
        {
            // Check argument manually for consistent behavior on both Silverlight and Windows runtimes
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(nameof(fileName) + " cannot be null or whitespace", nameof(fileName));
            }

            this.folder.Create();

            string fullFileName = Path.Combine(this.folder.FullName, fileName);
            var fileInfo = new FileInfo(fullFileName);
            if (fileInfo.Exists)
            {
                throw new IOException("Cannot create file '" + fileName + "' because it already exists.");
            }

            using (fileInfo.Create())
            {
            }

            // we need to re-create FileInfo because property Exist equal to false at the original object 
            // and in .NET Core there's an explicit check for Exists field in MoveTo() method and it throws FileNotFoundException if it's false
            return new PlatformFile(new FileInfo(fullFileName));
        }
    }
} 