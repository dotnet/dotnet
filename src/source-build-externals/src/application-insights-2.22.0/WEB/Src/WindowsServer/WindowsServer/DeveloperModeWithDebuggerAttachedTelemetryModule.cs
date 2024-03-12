namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Telemetry module that sets developer mode to true when is not already set AND managed debugger is attached.
    /// </summary>
    public class DeveloperModeWithDebuggerAttachedTelemetryModule : ITelemetryModule
    {
        /// <summary>
        /// Function that checks whether debugger is attached with implementation that can be replaced by unit test code.
        /// </summary>
        internal static Func<bool> IsDebuggerAttached = () => Debugger.IsAttached;

        /// <summary>
        /// Gives the opportunity for this telemetry module to initialize configuration object that is passed to it.
        /// </summary>
        /// <param name="configuration">Configuration object.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (!configuration.TelemetryChannel.DeveloperMode.HasValue && IsDebuggerAttached())
            {
                // Note that when debugger is not attached we are preserving default null value
                configuration.TelemetryChannel.DeveloperMode = true;
            }
        }
    }
}