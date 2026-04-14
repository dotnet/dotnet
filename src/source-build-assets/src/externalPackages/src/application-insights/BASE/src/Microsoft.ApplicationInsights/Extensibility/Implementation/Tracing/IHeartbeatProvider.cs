namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Internal interface for providing heartbeat information into the SDK pipeline. Useful
    /// for mocking for unit test purposes.
    /// </summary>
    internal interface IHeartbeatProvider : IHeartbeatPropertyManager, IDisposable
    {
        string InstrumentationKey { get; set; }

        void Initialize(TelemetryConfiguration configuration);

        bool AddHeartbeatProperty(string propertyName, bool overrideDefaultField, string propertyValue, bool isHealthy);

        bool SetHeartbeatProperty(string propertyName, bool overrideDefaultField, string propertyValue = null, bool? isHealthy = null);
    }
}
