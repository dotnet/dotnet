namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.ComponentModel;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Extension methods for TelemetryContext.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TelemetryContextExtensions
    {
        /// <summary>
        /// Returns TelemetryContext's Internal context.
        /// </summary>
        /// <param name="context">Telemetry context to get Internal context for.</param>
        /// <returns>Internal context for TelemetryContext.</returns>
        public static InternalContext GetInternalContext(this TelemetryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Internal;
        }
    }
}
