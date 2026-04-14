namespace Microsoft.ApplicationInsights.DataContracts
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an object that supports application-defined metrics.
    /// </summary>
    public interface ISupportMetrics
    {
        /// <summary>
        /// Gets a dictionary of application-defined metric names and values providing additional information about telemetry.
        /// </summary>
        IDictionary<string, double> Metrics { get; }
    }
}
