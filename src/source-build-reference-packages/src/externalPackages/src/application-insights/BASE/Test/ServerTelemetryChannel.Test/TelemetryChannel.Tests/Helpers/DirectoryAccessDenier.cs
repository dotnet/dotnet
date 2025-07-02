namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers
{
    using System;
    using System.IO;
    using System.Security.AccessControl;
    using System.Security.Principal;

    internal sealed class DirectoryAccessDenier : IDisposable
    {
        private readonly DirectoryInfo directory;
        private readonly DirectorySecurity access;
        private readonly FileSystemAccessRule denial;

        public DirectoryAccessDenier(DirectoryInfo directory, FileSystemRights rights)
        {
            this.directory = directory;
            this.access = this.directory.GetAccessControl();
            this.denial = new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name, rights, AccessControlType.Deny);
            this.access.AddAccessRule(this.denial);
            this.directory.SetAccessControl(this.access);
        }

        public void Dispose()
        {
            this.access.RemoveAccessRule(this.denial);
            this.directory.SetAccessControl(this.access);
        }
    }
}
