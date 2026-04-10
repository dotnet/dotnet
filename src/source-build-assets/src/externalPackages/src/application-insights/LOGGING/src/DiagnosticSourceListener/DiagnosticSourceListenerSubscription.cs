//-----------------------------------------------------------------------
// <copyright file="DiagnosticSourceListenerSubscription.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.DiagnosticSourceListener
{
    using System;
    using System.Collections.Generic;

    internal class DiagnosticSourceListenerSubscription : IObserver<KeyValuePair<string, object>>
    {
        private readonly string listenerName;
        private readonly TelemetryClient telemetryClient;
        private readonly OnEventWrittenHandler onEventWrittenHandler;

        public DiagnosticSourceListenerSubscription(string listenerName, TelemetryClient telemetryClient, OnEventWrittenHandler onEventWrittenHandler)
        {
            if (listenerName == null)
            {
                throw new ArgumentNullException(nameof(listenerName));
            }

            if (telemetryClient == null)
            {
                throw new ArgumentNullException(nameof(telemetryClient));
            }

            if (onEventWrittenHandler == null)
            {
                throw new ArgumentNullException(nameof(onEventWrittenHandler));
            }

            this.listenerName = listenerName;
            this.telemetryClient = telemetryClient;
            this.onEventWrittenHandler = onEventWrittenHandler;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        /// <summary>
        /// Observes an event from the diagnostic source and logs it as a message to Application Insights.
        /// </summary>
        /// <param name="event">The event (message and payload) from the diagnostic source.</param>
        public void OnNext(KeyValuePair<string, object> @event)
        {
            try
            {
                this.onEventWrittenHandler(this.listenerName, @event.Key, @event.Value, this.telemetryClient);
            }
            catch (Exception ex)
            {
                this.telemetryClient.TrackException(ex);
            }
        }
    }
}
