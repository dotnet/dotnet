namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers
{
    using System;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    
    internal class StubApplicationFolderProvider : IApplicationFolderProvider
    {
        public Func<IPlatformFolder> OnGetApplicationFolder = () => new StubPlatformFolder();

        public IPlatformFolder GetApplicationFolder()
        {
            return this.OnGetApplicationFolder();
        }
    }
}
