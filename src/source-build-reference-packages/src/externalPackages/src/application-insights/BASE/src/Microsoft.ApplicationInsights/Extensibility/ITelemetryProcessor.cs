namespace Microsoft.ApplicationInsights.Extensibility
{
    using Microsoft.ApplicationInsights.Channel;

    /// <summary>
    /// Represents an object used to process telemetry as part of sending it to Application Insights.
    /// </summary>
    public interface ITelemetryProcessor
    {
        /// <summary>
        /// Process a collected telemetry item.
        /// </summary>
        /// <param name="item">A collected Telemetry item.</param>
        void Process(ITelemetry item);        
    }
}
