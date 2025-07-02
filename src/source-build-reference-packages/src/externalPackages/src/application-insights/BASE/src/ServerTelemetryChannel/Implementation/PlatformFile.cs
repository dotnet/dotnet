namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.IO;

    internal class PlatformFile : IPlatformFile
    {
        private readonly FileInfo file;

        public PlatformFile(FileInfo file)
        {
            this.file = file ?? throw new ArgumentNullException(nameof(file));
        }

        public string Name
        {
            get { return this.file.Name; }
        }

        public string Extension
        {
            get { return this.file.Extension; }
        }

        public long Length
        {
            get { return this.file.Length; }
        }

        public DateTimeOffset DateCreated
        {
            get { return this.file.CreationTime; }
        }

        public bool Exists
        {
            get { return this.file.Exists; }
        }

        public void Delete()
        {
            if (!File.Exists(this.file.FullName))
            {
                throw new FileNotFoundException();
            }

            this.file.Delete();
        }

        public void Rename(string newName)
        {
            // Check argument manually for consistent behavior on both Silverlight and Windows runtimes
            if (newName == null)
            {
                throw new ArgumentNullException(nameof(newName));
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("cannot be null or whitespace", nameof(newName));
            }

            if (!File.Exists(this.file.FullName))
            {
                throw new FileNotFoundException("Could not find the file to rename.", this.file.Name);
            }

            this.file.MoveTo(Path.Combine(this.file.DirectoryName, newName));
        }

        public Stream Open()
        {
            return this.file.Open(FileMode.Open, FileAccess.ReadWrite);
        }
    }
}