namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.FileDiagnosticsModule
{
    using System.Diagnostics.Tracing;

    /// <summary>
    /// Interface for subscribing on EventSource.
    /// </summary>
    internal interface IEventListener
    {
        /// <summary>
        /// Sends diagnostics data to the appropriate output.
        /// </summary>
        /// <param name="eventData">Information about trace event.</param>
        void OnEventWritten(EventWrittenEventArgs eventData);
    }
}