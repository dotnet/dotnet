namespace Microsoft.ApplicationInsights.Extensibility
{
    /// <summary>
    /// Represents an object that supports initialization from <see cref="TelemetryConfiguration"/>.
    /// </summary>
    public interface ITelemetryModule
    {
        /// <summary>
        /// Initialize method is called after all configuration properties have been loaded from the configuration.
        /// </summary>
        void Initialize(TelemetryConfiguration configuration);
    }
}
