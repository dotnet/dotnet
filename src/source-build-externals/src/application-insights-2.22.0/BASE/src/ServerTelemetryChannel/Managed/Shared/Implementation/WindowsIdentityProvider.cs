namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Shared.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;

    internal class WindowsIdentityProvider : IIdentityProvider
    {
        string IIdentityProvider.GetName()
        {
            return WindowsIdentity.GetCurrent().Name;
        }
    }
}
