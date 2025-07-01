#if AI_ASPNETCORE_WEB
    namespace Microsoft.ApplicationInsights.AspNetCore
#else
    namespace Microsoft.ApplicationInsights.WorkerService
#endif
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// No-op telemetry module that is added instead of actual one, when the actual module is disabled.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "This class is instantiated by Dependency Injection.")]
    internal class NoOpTelemetryModule : ITelemetryModule
    {
        /// <summary>
        /// This is a no-op and will do nothing.
        /// </summary>
        /// <param name="configuration">This does nothing.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
        }
    }
}
