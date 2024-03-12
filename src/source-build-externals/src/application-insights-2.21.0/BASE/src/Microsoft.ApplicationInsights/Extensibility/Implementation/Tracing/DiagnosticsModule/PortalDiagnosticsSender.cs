namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// This class is responsible for sending diagnostics information into portal.
    /// </summary>
    internal class PortalDiagnosticsSender : IDiagnosticsSender
    {
        private const string SdkTelemetrySyntheticSourceName = "SDKTelemetry";

        private readonly TelemetryClient telemetryClient;
        private readonly IDiagnoisticsEventThrottlingManager throttlingManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortalDiagnosticsSender"/> class. 
        /// </summary>
        public PortalDiagnosticsSender(
            TelemetryConfiguration configuration,
            IDiagnoisticsEventThrottlingManager throttlingManager)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (throttlingManager == null)
            {
                throw new ArgumentNullException(nameof(throttlingManager));
            }

            this.telemetryClient = new TelemetryClient(configuration);

            this.throttlingManager = throttlingManager;
        }

        /// <summary>
        /// Gets or sets instrumentation key for diagnostics (optional).
        /// </summary>
        public string DiagnosticsInstrumentationKey { get; set; }

        public void Send(TraceEvent eventData)
        {
            try
            {
                if (eventData.MetaData != null && !string.IsNullOrEmpty(eventData.MetaData.MessageFormat))
                {
                    // Check if trace message is sent to the portal (somewhere before in the stack)
                    // It allows to avoid infinite recursion if sending to the portal traces something.
                    if (!ThreadResourceLock.IsResourceLocked)
                    {
                        using (var portalSenderLock = new ThreadResourceLock())
                        {
                            try
                            {
                                if (!this.throttlingManager.ThrottleEvent(eventData.MetaData.EventId, eventData.MetaData.Keywords))
                                {
                                    this.InternalSendTraceTelemetry(eventData);
                                }
                            }
                            catch (Exception exp)
                            {
                                // This message will not be sent to the portal because we have infinite loop protection
                                // But it will be available in PerfView or StatusMonitor
                                CoreEventSource.Log.LogError("Failed to send traces to the portal: " + exp.ToInvariantString());
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // We were trying to send traces out and failed. 
                // No reason to try to trace something else again
            }
        }

        private void InternalSendTraceTelemetry(TraceEvent eventData)
        {
            if (this.telemetryClient.TelemetryConfiguration.TelemetryChannel == null)
            {
                return;
            }

            var traceTelemetry = new TraceTelemetry
            {
                Message = eventData.ToString(),
            };

            if (!string.IsNullOrEmpty(this.DiagnosticsInstrumentationKey))
            {
                traceTelemetry.Context.InstrumentationKey = this.DiagnosticsInstrumentationKey;
            }

            traceTelemetry.Context.Operation.SyntheticSource = SdkTelemetrySyntheticSourceName;

            this.telemetryClient.TrackTrace(traceTelemetry);
        }
    }
}
