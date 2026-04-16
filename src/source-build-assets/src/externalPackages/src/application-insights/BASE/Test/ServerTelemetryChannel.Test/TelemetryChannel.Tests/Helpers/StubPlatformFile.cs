namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers
{
    using System;
    using System.IO;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using TestFramework;

    using TaskEx = System.Threading.Tasks.Task;

    internal class StubPlatformFile : IPlatformFile
    {
        public Func<string> OnGetName;
        public Func<DateTimeOffset> OnGetDateCreated = () => DateTimeOffset.Now;
        public Func<long> OnGetLength;
        public Action OnDelete = () => { };
        public Func<Stream> OnOpen;
        public Action<string> OnRename;

        private readonly StubStream stream = new StubStream { OnDispose = disposing => { /* don't dispose */ } };
        private string name;

        public StubPlatformFile(string name = null)
        {
            this.name = name ?? string.Empty;

            this.OnGetName = () => this.name;
            this.OnGetLength = () => this.stream.Length;
            this.OnOpen = () =>
            {
                this.stream.Seek(0, SeekOrigin.Begin);
                return this.stream;
            };
            this.OnRename = desiredName =>
            {
                this.name = desiredName;
            };
        }

        public string Name
        {
            get { return this.OnGetName(); }
        }

        public string Extension
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Name))
                {
                    return Path.GetExtension(this.Name);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public long Length
        {
            get
            {
                return this.OnGetLength();
            }
        }

        public DateTimeOffset DateCreated
        {
            get { return this.OnGetDateCreated(); }
        }

        public bool Exists
        {
            get { return true; }
        }

        public void Delete()
        {
            this.OnDelete();
        }

        public Stream Open()
        {
            return this.OnOpen();
        }

        public void Rename(string newFileName)
        {
            this.OnRename(newFileName);
        }
    }
}
