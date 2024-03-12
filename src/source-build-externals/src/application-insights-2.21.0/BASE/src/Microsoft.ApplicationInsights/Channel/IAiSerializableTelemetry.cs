namespace Microsoft.ApplicationInsights.Channel
{
    /// <summary>
    /// This interface defines Telemetry objects that are intended to be serialized for the Application Insights Ingestion Endpoint.
    /// </summary>
    internal interface IAiSerializableTelemetry
    {
        /// <summary>
        /// Gets or sets the name of the Telemetry. Used internally for serialization.
        /// </summary>
        string TelemetryName { get; set; }

        /// <summary>
        /// Gets the name of the TelemetryType. Typically the datatype of the internal Data property. Used internally for serialization.
        /// </summary>
        string BaseType { get; }
    }
}
