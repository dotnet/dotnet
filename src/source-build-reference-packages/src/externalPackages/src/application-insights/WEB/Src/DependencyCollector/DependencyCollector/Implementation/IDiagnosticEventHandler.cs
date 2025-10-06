namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// DiagnosticsSource events handler.
    /// </summary>
    internal interface IDiagnosticEventHandler
    {
        /// <summary>
        /// Handles event and tracks telemetry if needed.
        /// </summary>
        /// <param name="evnt">The event.</param>
        /// <param name="diagnosticListener">DiagnosticListener that sent this event.</param>
        void OnEvent(KeyValuePair<string, object> evnt, DiagnosticListener diagnosticListener);

        /// <summary>
        /// Checks if the event is enabled by this listener.
        /// </summary>
        /// <param name="evnt">The event name.</param>
        /// <param name="arg1">First event input object (<see cref="DiagnosticListener.IsEnabled(string, object, object)"/>).</param>
        /// <param name="arg2">Second event input object (<see cref="DiagnosticListener.IsEnabled(string, object, object)"/>).</param>
        bool IsEventEnabled(string evnt, object arg1, object arg2);
    }
}
