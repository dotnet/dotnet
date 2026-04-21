namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using Microsoft.ApplicationInsights.Common;

#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-Extensibility-PerformanceCollector")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-Extensibility-PerformanceCollector")]
#endif
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed class PerformanceCollectorEventSource : EventSource
    {
        private readonly ApplicationNameProvider applicationNameProvider = new ApplicationNameProvider();

        private PerformanceCollectorEventSource()
        {
        }

        public static PerformanceCollectorEventSource Log { get; } = new PerformanceCollectorEventSource();

        #region Infra init - success

        [Event(1, Level = EventLevel.Informational, Message = @"Performance counter infrastructure is being initialized. {0}")]
        public void ModuleIsBeingInitializedEvent(
            string message,
            string applicationName = "dummy")
        {
            this.WriteEvent(1, message, this.applicationNameProvider.Name);
        }

        [Event(3, Level = EventLevel.Informational, Message = @"Performance counter {0} has been successfully registered with performance collector.")]
        public void CounterRegisteredEvent(string counter, string applicationName = "dummy")
        {
            this.WriteEvent(3, counter, this.applicationNameProvider.Name);
        }

        [Event(4, Level = EventLevel.Informational, Message = @"Performance counters have been refreshed. Refreshed counters count is {0}.")]
        public void CountersRefreshedEvent(
            string countersRefreshedCount,
            string applicationName = "dummy")
        {
            this.WriteEvent(4, countersRefreshedCount, this.applicationNameProvider.Name);
        }

#endregion

#region Infra init - failure

        [Event(5, Keywords = Keywords.UserActionable, Level = EventLevel.Warning, Message = @"Performance counter {1} has failed to register with performance collector. Please make sure it exists. Technical details: {0}")]
        public void CounterRegistrationFailedEvent(string e, string counter, string applicationName = "dummy")
        {
            this.WriteEvent(5, e, counter, this.applicationNameProvider.Name);
        }

        [Event(6, Keywords = Keywords.UserActionable, Level = EventLevel.Warning, Message = @"Performance counter specified as {1} in ApplicationInsights.config was not parsed correctly. Please make sure that a proper name format is used. Expected formats are \category(instance)\counter or \category\counter. Technical details: {0}")]
        public void CounterParsingFailedEvent(string e, string counter, string applicationName = "dummy")
        {
            this.WriteEvent(6, e, counter, this.applicationNameProvider.Name);
        }

        [Event(8, Keywords = Keywords.UserActionable, Level = EventLevel.Error,
            Message = @"Error collecting {0} of the configured performance counters. Please check the configuration.
{1}")]
        public void CounterCheckConfigurationEvent(
            string misconfiguredCountersCount,
            string e,
            string applicationName = "dummy")
        {
            this.WriteEvent(8, misconfiguredCountersCount, e, this.applicationNameProvider.Name);
        }

        // Verbosity is Error - so it is always sent to portal; Keyword is Diagnostics so throttling is not applied.
        [Event(15, Level = EventLevel.Error,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable,
            Message = @"Diagnostic message: Performance counters are unavailable when the application is running under IIS Express. Use EnableIISExpressPerformanceCounters element with a value of 'true' within the Performance Collector Module element to override this behavior.")]
        public void RunningUnderIisExpress(string applicationName = "dummy")
        {
            this.WriteEvent(15, this.applicationNameProvider.Name);
        }

#endregion

#region Data reading - success

        [Event(9, Level = EventLevel.Verbose, Message = @"About to perform counter collection...")]
        public void CounterCollectionAttemptEvent(string applicationName = "dummy")
        {
            this.WriteEvent(9, this.applicationNameProvider.Name);
        }

        [Event(10, Level = EventLevel.Verbose, Message = @"Counters successfully collected. Counter count: {0}, collection time: {1}")]
        public void CounterCollectionSuccessEvent(
            long counterCount,
            long operationDurationInMs,
            string applicationName = "dummy")
        {
            this.WriteEvent(10, counterCount, operationDurationInMs, this.applicationNameProvider.Name);
        }

#endregion

#region Data reading - failure

        [Event(11, Level = EventLevel.Warning, Message = @"Performance counter {1} has failed the reading operation. Error message: {0}")]
        public void CounterReadingFailedEvent(string e, string counter, string applicationName = "dummy")
        {
            this.WriteEvent(11, e, counter, this.applicationNameProvider.Name);
        }

#endregion

#region Data sending - success

#endregion

#region Data sending - failure

        [Event(12, Level = EventLevel.Warning, Message = @"Failed to send a telemetry item for performance collector. Error text: {0}")]
        public void TelemetrySendFailedEvent(string e, string applicationName = "dummy")
        {
            this.WriteEvent(12, e, this.applicationNameProvider.Name);
        }

#endregion

#region Unknown errors

        [Event(13, Keywords = Keywords.UserActionable, Level = EventLevel.Warning, Message = @"Unknown error in performance counter infrastructure: {0}")]
        public void UnknownErrorEvent(string e, string applicationName = "dummy")
        {
            this.WriteEvent(13, e, this.applicationNameProvider.Name);
        }

#endregion

#region Troubleshooting

        [Event(14, Message = "{0}", Level = EventLevel.Verbose)]
        public void TroubleshootingMessageEvent(string message, string applicationName = "dummy")
        {
            this.WriteEvent(14, message, this.applicationNameProvider.Name);
        }

        [Event(16, Keywords = Keywords.UserActionable, Level = EventLevel.Error, Message = @"Performance counter is not available in the web app supported list. Counter is {0}.")]
        public void CounterNotWebAppSupported(
            string counterName,
            string applicationName = "dummy")
        {
            this.WriteEvent(16, counterName, this.applicationNameProvider.Name);
        }

        [Event(17, Level = EventLevel.Warning, Message = @"Accessing environment variable - {0} failed with exception: {1}.")]
        public void AccessingEnvironmentVariableFailedWarning(
            string environmentVariable,
            string exceptionMessage,
            string applicationName = "dummy")
        {
            this.WriteEvent(17, environmentVariable, exceptionMessage, this.applicationNameProvider.Name);
        }

        [Event(18, Keywords = Keywords.UserActionable, Level = EventLevel.Warning, Message = @"Web App Performance counter {1} has failed to register with performance collector. Please make sure it exists. Technical details: {0}")]
        public void WebAppCounterRegistrationFailedEvent(string e, string counter, string applicationName = "dummy")
        {
            this.WriteEvent(18, e, counter, this.applicationNameProvider.Name);
        }

        [Event(19, Level = EventLevel.Error, Message = @"CounterName:{2} has unexpected (negative) value. Last Collected Value:{0} Previous Value:{1}. To avoid negative value, this will be reported as zero instead.")]
        public void WebAppCounterNegativeValue(double lastCollectedValue, double previouslyCollectedValue, string counterName, string applicationName = "dummy")
        {
            this.WriteEvent(19, lastCollectedValue, previouslyCollectedValue, counterName, this.applicationNameProvider.Name);
        }

        [Event(20, Level = EventLevel.Error, Message = @"Processors count has incorrect value: {0}. Normalized process CPU counter value will be reported as 0.")]
        public void ProcessorsCountIncorrectValueError(string count, string applicationName = "dummy")
        {
            this.WriteEvent(20, count, this.applicationNameProvider.Name);
        }

        [Event(21, Level = EventLevel.Informational, Message = @"PerfCounter collection is supported for Apps targetting .NET Core only when they are deployed as Azure Web Apps")]
        public void PerfCounterNetCoreOnlyOnAzureWebApp(string applicationName = "dummy")
        {
            this.WriteEvent(21, this.applicationNameProvider.Name);
        }

        [Event(22, Keywords = Keywords.UserActionable, Level = EventLevel.Error, Message = @"Performance counter is not available in the supported list of XPlatform counters. Counter is {0}.")]
        public void CounterNotXPlatformSupported(
    string counterName,
    string applicationName = "dummy")
        {
            this.WriteEvent(22, counterName, this.applicationNameProvider.Name);
        }

        [Event(23, Level = EventLevel.Informational, Message = @"PerformanceCollector is: {0}.")]
        public void InitializedWithCollector(
            string collectorName,
            string applicationName = "dummy")
        {
            this.WriteEvent(23, collectorName, this.applicationNameProvider.Name);
        }

        #endregion

        public class Keywords
        {
            public const EventKeywords UserActionable = (EventKeywords)0x1;

            public const EventKeywords Diagnostics = (EventKeywords)0x2;
        }
    }
}