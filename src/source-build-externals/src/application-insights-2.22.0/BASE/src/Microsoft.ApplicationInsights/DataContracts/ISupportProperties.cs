namespace Microsoft.ApplicationInsights.DataContracts
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an object that supports application-defined properties.
    /// </summary>
    public interface ISupportProperties
    {
        /// <summary>
        /// Gets a dictionary of application-defined property names and values providing additional information about telemetry.
        /// </summary>
        IDictionary<string, string> Properties { get; }
    }
}
