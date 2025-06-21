namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using Microsoft.ApplicationInsights.Channel;

    /// <summary>
    /// Extensions for ITelemetry interface.
    /// </summary>
    public static class TelemetryExtensions
    {
        private const string DefaultEnvelopeName = "Event";

        /// <summary>
        /// Sets envelope name for ITelemetry object.
        /// </summary>
        /// <param name="telemetry">ITelemetry object to set envelope name for.</param>
        /// <param name="envelopeName">Envelope name to use for ITelemetry object.</param>
        /// <returns>Boolean indicating the success of assigning envelope name.</returns>
        public static bool TrySetEnvelopeName(this ITelemetry telemetry, string envelopeName)
        {
            if (telemetry is IAiSerializableTelemetry aiSerializableTelemetry)
            {
                aiSerializableTelemetry.TelemetryName = envelopeName;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets envelope name for ITelemetry object.
        /// </summary>
        /// <param name="telemetry">ITelemetry object to set envelope name for.</param>
        /// <returns>Envelope name of the provided ITelemetry object.</returns>
        public static string GetEnvelopeName(this ITelemetry telemetry)
        {
            if (telemetry is IAiSerializableTelemetry aiSerializableTelemetry)
            {
                return aiSerializableTelemetry.TelemetryName;
            }
            else
            {
                return DefaultEnvelopeName;
            }
        }
    }
}
