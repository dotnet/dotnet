//-----------------------------------------------------------------------
// <copyright file="DiagnosticSourceTelemetryModule.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.DiagnosticSourceListener
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Implementation;

    /// <summary>
    /// Delegate to apply custom formatting to Application Insights trace telemetry from the Diagnostics Source data.
    /// </summary>
    /// <param name="sourceName">Name of the DiagnosticsSource (as implemented by a DiagnosticsListener).</param>
    /// <param name="message">Name of the event.</param>
    /// <param name="payload">Data associated with the event.</param>
    /// <param name="client">Telemetry client to report telemetry to.</param>
    public delegate void OnEventWrittenHandler(string sourceName, string message, object payload, TelemetryClient client);

    /// <summary>
    /// A module to forward diagnostic source events to Application Insights.
    /// </summary>
    public sealed class DiagnosticSourceTelemetryModule : IObserver<DiagnosticListener>, ITelemetryModule, IDisposable
    {
        private readonly OnEventWrittenHandler onEventWrittenHandler;

        private TelemetryClient client;
        private IDisposable allDiagnosticListenersSubscription;
        private List<IDisposable> diagnosticListenerSubscriptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticSourceTelemetryModule"/> class.
        /// </summary>
        public DiagnosticSourceTelemetryModule() : this(Track)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticSourceTelemetryModule"/> class.
        /// </summary>
        /// <param name="onEventWrittenHandler">Action to be executed each time an event is written to format and send via the configured <see cref="TelemetryClient"/>.</param>
        public DiagnosticSourceTelemetryModule(OnEventWrittenHandler onEventWrittenHandler)
        {
            if (onEventWrittenHandler == null)
            {
                throw new ArgumentNullException(nameof(onEventWrittenHandler));
            }

            this.onEventWrittenHandler = onEventWrittenHandler;
        }

        /// <summary>
        /// Gets the list of DiagnosticSource listening requests (information about which DiagnosticSources should be traced).
        /// </summary>
        public IList<DiagnosticSourceListeningRequest> Sources { get; } = new List<DiagnosticSourceListeningRequest>();

        /// <summary>
        /// Initializes the telemetry module and starts tracing DiagnosticSources specified via the <see cref="Sources"/> property.
        /// </summary>
        /// <param name="configuration">The configuration object for the Application Insights pipeline.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var telemetryClient = new TelemetryClient(configuration);
            telemetryClient.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("dsl:");

            this.client = telemetryClient;

            // Protect against multiple subscriptions if Initialize is called twice
            if (this.allDiagnosticListenersSubscription == null)
            {
                var subscription = DiagnosticListener.AllListeners.Subscribe(this);
                if (Interlocked.CompareExchange(ref this.allDiagnosticListenersSubscription, subscription, null) != null)
                {
                    subscription.Dispose();
                }
            }
        }

        /// <summary>
        /// Dispose of this instance.
        /// </summary>
        public void Dispose()
        {
            if (this.diagnosticListenerSubscriptions != null)
            {
                foreach (var subscription in this.diagnosticListenerSubscriptions)
                {
                    subscription.Dispose();
                }
            }

            this.allDiagnosticListenersSubscription?.Dispose();
        }

        void IObserver<DiagnosticListener>.OnCompleted()
        {
        }

        void IObserver<DiagnosticListener>.OnError(Exception error)
        {
        }

        void IObserver<DiagnosticListener>.OnNext(DiagnosticListener listener)
        {
            foreach (var source in this.Sources)
            {
                if (source.Name == listener.Name)
                {
                    if (this.diagnosticListenerSubscriptions == null)
                    {
                        this.diagnosticListenerSubscriptions = new List<IDisposable>();
                    }

                    var subscription = new DiagnosticSourceListenerSubscription(listener.Name, this.client, this.onEventWrittenHandler);
                    this.diagnosticListenerSubscriptions.Add(listener.Subscribe(subscription));
                    break;
                }
            }
        }

        private static void Track(string sourceName, string message, object payload, TelemetryClient client)
        {
            var telemetry = new TraceTelemetry(message, SeverityLevel.Information);
            telemetry.Properties.Add("DiagnosticSource", sourceName);

            // Transfer properties from payload to telemetry
            if (payload != null)
            {
                foreach (var property in DeclaredPropertiesCache.GetDeclaredProperties(payload))
                {
                    if (!property.IsSpecialName)
                    {
                        telemetry.Properties.Add(property.Name, Convert.ToString(property.GetValue(payload), CultureInfo.InvariantCulture));
                    }
                }
            }

            client.TrackTrace(telemetry);
        }
    }
}
