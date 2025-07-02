namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Shared.Implementation
{
    using System;

    internal class NonWindowsIdentityProvider : IIdentityProvider
    {
        string IIdentityProvider.GetName()
        {
            return Environment.UserName;
        }
    }
}
