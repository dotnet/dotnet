namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;

    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    internal static class HeartbeatPropertyManagerProvider
    {
        public static IHeartbeatPropertyManager GetHeartbeatPropertyManager()
        {
            var telemetryModules = TelemetryModules.Instance;
            if (telemetryModules != null)
            {
                try
                {
                    foreach (var module in telemetryModules.Modules)
                    {
                        if (module is IHeartbeatPropertyManager hman)
                        {
                            return hman;
                        }
                    }
                }
                catch (Exception hearbeatManagerAccessException)
                {
                    WindowsServerEventSource.Log.AppServiceHeartbeatManagerAccessFailure(hearbeatManagerAccessException.ToInvariantString());
                }
            }

            // Module was not found. Log and return null.
            WindowsServerEventSource.Log.AppServiceHeartbeatManagerNotAvailable();
            return null;
        }
    }
}
