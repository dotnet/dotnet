namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers
{
    using System;
    using System.IO;
    using System.Security.AccessControl;
    using System.Security.Principal;

    internal sealed class FileAccessDenier : IDisposable
    {
        private readonly FileInfo file;
        private readonly FileSecurity access;
        private readonly FileSystemAccessRule denial;

        public FileAccessDenier(FileInfo file, FileSystemRights rights)
        {
            this.file = file;
            this.access = this.file.GetAccessControl();
            this.denial = new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name, rights, AccessControlType.Deny);
            this.access.AddAccessRule(this.denial);
            this.file.SetAccessControl(this.access);
        }

        public void Dispose()
        {
            this.access.RemoveAccessRule(this.denial);
            this.file.SetAccessControl(this.access);
        }
    }
}
