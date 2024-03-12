namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Shared.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal interface IIdentityProvider
    {
        string GetName();
    }
}
