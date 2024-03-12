#if AI_ASPNETCORE_WEB
    namespace Microsoft.ApplicationInsights.AspNetCore
#else
namespace Microsoft.ApplicationInsights.WorkerService
#endif
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Represents factory used to create <see cref="ITelemetryProcessor"/> with dependency injection support.
    /// </summary>
    public interface ITelemetryProcessorFactory
    {
        /// <summary>
        /// Creates an instance of the telemetry processor, passing the
        /// next <see cref="ITelemetryProcessor"/> in the call chain to
        /// its constructor.
        /// </summary>
        /// <param name="nextProcessor">The next processor in the chain.</param>
        /// <returns>Returns a new TelemetryProcessor with it's Next property set to the provided processor.</returns>
        ITelemetryProcessor Create(ITelemetryProcessor nextProcessor);
    }
}