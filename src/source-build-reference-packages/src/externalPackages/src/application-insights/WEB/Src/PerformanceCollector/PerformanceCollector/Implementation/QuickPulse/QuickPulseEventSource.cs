namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using System.Reflection;
    using Microsoft.ApplicationInsights.Common;

#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-Extensibility-PerformanceCollector-QuickPulse")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-Extensibility-PerformanceCollector-QuickPulse")]
#endif
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed class QuickPulseEventSource : EventSource
    {
        private static readonly QuickPulseEventSource Logger = new QuickPulseEventSource();
        private readonly ApplicationNameProvider applicationNameProvider = new ApplicationNameProvider();

        private QuickPulseEventSource()
        {
        }

        public static QuickPulseEventSource Log
        {
            get
            {
                return Logger;
            }
        }

        #region Infra init - success

        [Event(1, Level = EventLevel.Informational, Message = @"QuickPulse infrastructure is being initialized. QuickPulseServiceEndpoint: '{0}', DisableFullTelemetryItems: '{1}', DisableTopCpuProcesses: '{2}', AuthApiKey: '{3}' ")]
        public void ModuleIsBeingInitializedEvent(
            string serviceEndpoint,
            bool disableFullTelemetryItems,
            bool disableTopCpuProcesses,
            string authApiKey,
            string applicationName = "dummy")
        {
            this.WriteEvent(1, serviceEndpoint ?? string.Empty, disableFullTelemetryItems, disableTopCpuProcesses, authApiKey, this.applicationNameProvider.Name);
        }

        [Event(3, Level = EventLevel.Informational, Message = @"Performance counter {0} has been successfully registered with QuickPulse performance collector.")]
        public void CounterRegisteredEvent(string counter, string applicationName = "dummy")
        {
            this.WriteEvent(3, counter ?? string.Empty, this.applicationNameProvider.Name);
        }
        #endregion

        #region Infra init - failure
        [Event(5, Keywords = Keywords.UserActionable, Level = EventLevel.Error, Message = @"Performance counter {1} has failed to register with QuickPulse performance collector. This might happen whenever an application is running on a platform that doesn't provide access to performance counters. Technical details: {0}")]
        public void CounterRegistrationFailedEvent(string e, string counter, string applicationName = "dummy")
        {
            this.WriteEvent(5, e ?? string.Empty, counter ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(6, Level = EventLevel.Warning, Message = @"Performance counter specified in QuickPulse as {1} was not parsed correctly. Technical details: {0}")]
        public void CounterParsingFailedEvent(string e, string counter, string applicationName = "dummy")
        {
            this.WriteEvent(6, e ?? string.Empty, counter ?? string.Empty, this.applicationNameProvider.Name);
        }
        #endregion

        #region Data reading - success

        #endregion

        #region Data reading - failure
        [Event(11, Level = EventLevel.Warning, Message = @"Performance counter {1} has failed the reading operation in QuickPulse. Error message: {0}")]
        public void CounterReadingFailedEvent(string e, string counter, string applicationName = "dummy")
        {
            this.WriteEvent(11, e ?? string.Empty, counter ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(20, Level = EventLevel.Verbose, Message = @"QuickPulse has failed to read process information. Error message: {0}")]
        public void ProcessesReadingFailedEvent(string e, string applicationName = "dummy")
        {
            this.WriteEvent(20, e ?? string.Empty, this.applicationNameProvider.Name);
        }
        #endregion

        #region Data sending - success

        #endregion

        #region Data sending - failure
        [Event(12, Level = EventLevel.Verbose, Message = @"Failed to communicate with the QuickPulse service. Error text: {0}")]
        public void ServiceCommunicationFailedEvent(string e, string applicationName = "dummy")
        {
            this.WriteEvent(12, e ?? string.Empty, this.applicationNameProvider.Name);
        }
        #endregion

        #region Unknown errors

        [Event(13, Level = EventLevel.Error, Message = @"Unexpected error in QuickPulse infrastructure: {0}. QuickPulse data will not be available.")]
        public void UnknownErrorEvent(string e, string applicationName = "dummy")
        {
            this.WriteEvent(13, e ?? string.Empty, this.applicationNameProvider.Name);
        }

        #endregion

        #region Troubleshooting

        [Event(14, Message = "{0}", Level = EventLevel.Verbose)]
        public void TroubleshootingMessageEvent(string message, string applicationName = "dummy")
        {
            this.WriteEvent(14, message ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(21, Message = "Waited for samples to cool down. {0}", Level = EventLevel.Verbose)]
        public void CollectionConfigurationSampleCooldownEvent(bool cooledDown, string applicationName = "dummy")
        {
            this.WriteEvent(21, cooledDown, this.applicationNameProvider.Name);
        }

        [Event(15, Message = "Sample submitted. Outgoing etag: '{0}'. Incoming etag: '{1}'. Response: '{2}'", Level = EventLevel.Verbose)]
        public void SampleSubmittedEvent(string outgoingEtag, string incomingEtag, string response, string applicationName = "dummy")
        {
            this.WriteEvent(15, outgoingEtag ?? string.Empty, incomingEtag ?? string.Empty, response ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(16, Message = "Ping sent. Outgoing etag: '{0}'. Incoming etag: '{1}'. Response: '{2}'", Level = EventLevel.Verbose)]
        public void PingSentEvent(string outgoingEtag, string incomingEtag, string response, string applicationName = "dummy")
        {
            this.WriteEvent(16, outgoingEtag ?? string.Empty, incomingEtag ?? string.Empty, response ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(17, Message = "State timer tick finished: {0} ms", Level = EventLevel.Verbose)]
        public void StateTimerTickFinishedEvent(long elapsedMs, string applicationName = "dummy")
        {
            this.WriteEvent(17, elapsedMs, this.applicationNameProvider.Name);
        }

        [Event(18, Message = "Collection timer tick finished: {0} ms", Level = EventLevel.Verbose)]
        public void CollectionTimerTickFinishedEvent(long elapsedMs, string applicationName = "dummy")
        {
            this.WriteEvent(18, elapsedMs, this.applicationNameProvider.Name);
        }

        [Event(19, Message = "Sample stored. Buffer length: {0}", Level = EventLevel.Verbose)]
        public void SampleStoredEvent(int bufferLength, string applicationName = "dummy")
        {
            this.WriteEvent(19, bufferLength, this.applicationNameProvider.Name);
        }

        [Event(7, Level = EventLevel.Verbose, Message = @"QuickPulseTelemetryModule has received a registration request from a QuickPulseTelemetryProcessor.")]
        public void ProcessorRegistered(string count, string applicationName = "dummy")
        {
            this.WriteEvent(7, count, this.applicationNameProvider.Name);
        }

        [Event(22, Message = "Collection configuration is being updated. Old etag: '{0}'. New etag: '{1}'. Configuration: '{2}'", Level = EventLevel.Verbose)]
        public void CollectionConfigurationUpdating(string oldEtag, string newEtag, string configuration, string applicationName = "dummy")
        {
            this.WriteEvent(22, oldEtag ?? string.Empty, newEtag ?? string.Empty, configuration ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(23, Message = "Collection configuration failed to update. Old etag: '{0}'. New etag: '{1}'. Configuration: '{2}'. Exception: '{3}'", Level = EventLevel.Verbose)]
        public void CollectionConfigurationUpdateFailed(string oldEtag, string newEtag, string configuration, string e, string applicationName = "dummy")
        {
            this.WriteEvent(23, oldEtag ?? string.Empty, newEtag ?? string.Empty, configuration ?? string.Empty, e ?? string.Empty, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an error indicating that the QuickPulseServiceClient could not acquire an auth token.
        /// Full exception is logged in the Base SDK at <see cref="Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.CoreEventSource.FailedToGetToken"/>.
        /// </summary>
        [Event(24, Message = "QuickPulse failed to get an auth token. Check 'CoreEventSource.FailedToGetToken' for full exception.", Level = EventLevel.Error)]
        public void FailedToGetAuthToken(string applicationName = "dummy")
        {
            this.WriteEvent(24, this.applicationNameProvider.Name);
        }

        #endregion

        public class Keywords
        {
            public const EventKeywords UserActionable = (EventKeywords)0x1;
        }
    }
}