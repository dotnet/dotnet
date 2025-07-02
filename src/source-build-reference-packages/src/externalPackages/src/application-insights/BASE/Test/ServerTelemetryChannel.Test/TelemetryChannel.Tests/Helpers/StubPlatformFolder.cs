namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

    internal class StubPlatformFolder : IPlatformFolder
    {
        public Func<string, IPlatformFile> OnCreateFile;
        public Action OnDelete;
        public Func<bool> OnExists;
        public Func<IEnumerable<IPlatformFile>> OnGetFiles;

        private readonly List<IPlatformFile> files = new List<IPlatformFile>();

        public StubPlatformFolder()
        {
            this.OnExists = () => true;
            this.OnDelete = () => { };
            this.OnGetFiles = () => this.files;
            this.OnCreateFile = name =>
            {
                var file = new StubPlatformFile(name);
                this.files.Add(file);
                return file;
            };
        }

        public string Name
        {
            get { return string.Empty; }
        }

        public IPlatformFile CreateFile(string fileName)
        {
            return this.OnCreateFile(fileName);
        }

        public void Delete()
        {
            this.OnDelete();
        }

        public bool Exists()
        {
            return this.OnExists();
        }

        IEnumerable<IPlatformFile> IPlatformFolder.GetFiles()
        {
            return this.OnGetFiles();
        }
    }
}
