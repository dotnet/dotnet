namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System.Collections.Generic;
    
    internal interface IPlatformFolder
    {
        string Name { get; }

        void Delete();

        bool Exists();

        IEnumerable<IPlatformFile> GetFiles();

        IPlatformFile CreateFile(string fileName);
    }
}
