namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// ETW EventSource tracing class.
    /// </summary>
#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-Extensibility-DependencyCollector")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-Extensibility-DependencyCollector")]
#endif
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed class DependencyCollectorEventSource : EventSource
    {
        public static readonly DependencyCollectorEventSource Log = new DependencyCollectorEventSource();
        private readonly ApplicationNameProvider applicationNameProvider = new ApplicationNameProvider();

        private DependencyCollectorEventSource()
        {
        }

        [Event(
            1,
            Keywords = Keywords.RddEventKeywords,
            Message = "[msg=RemoteDependencyModule failed];[msg={0}];[fwv={1}];",
            Level = EventLevel.Error)]
        public void RemoteDependencyModuleError(string msg, string frameworkVersion, string appDomainName = "Incorrect")
        {
            this.WriteEvent(1, msg ?? string.Empty, frameworkVersion ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            2,
            Keywords = Keywords.RddEventKeywords,
            Message = "[msg=DependencyOperationTelemetryInitializerFailed];[msg={0}]",
            Level = EventLevel.Error)]
        public void DependencyOperationTelemetryInitializerError(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(2, msg, this.applicationNameProvider.Name);
        }

        [Event(
            3,
            Keywords = Keywords.RddEventKeywords,
            Message = "[msg=DependencyOperationNameNull];",
            Level = EventLevel.Warning)]
        public void DependencyOperationNameNullWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(3, this.applicationNameProvider.Name);
        }

        [Event(
            4,
            Keywords = Keywords.RddEventKeywords,
            Message = "[msg=Runtime Dependency monitoring is turned off because APMC monitoring is enabled for the process. To enable AIC Runtime Dependency monitoring, turned off APMC monitoring for the current process.];",
            Level = EventLevel.Verbose)]
        public void RemoteDependencyDisabledApmcEnabled(string appDomainName = "Incorrect")
        {
            this.WriteEvent(4, this.applicationNameProvider.Name);
        }

        [Event(
            5,
            Keywords = Keywords.RddEventKeywords,
            Message = "[msg=RemoteDependencyModule warning.];[msg={0}]",
            Level = EventLevel.Warning)]
        public void RemoteDependencyModuleWarning(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(5, msg ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            6,
            Keywords = Keywords.RddEventKeywords,
            Message = "[msg=RemoteDependencyModule verbose.];[msg={0}]",
            Level = EventLevel.Verbose)]
        public void RemoteDependencyModuleVerbose(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(6, msg, this.applicationNameProvider.Name);
        }

        [Event(
            7,
            Keywords = Keywords.RddEventKeywords,
            Message = "[msg=RemoteDependencyTelemetryCollected.];",
            Level = EventLevel.Verbose)]
        public void RemoteDependencyTelemetryCollected(string appDomainName = "Incorrect")
        {
            this.WriteEvent(7, this.applicationNameProvider.Name);
        }

        [Event(
            8,
            Keywords = Keywords.RddEventKeywords,
            Message = "[msg=RemoteDependencyModule information.];[msg={0}]",
            Level = EventLevel.Informational)]
        public void RemoteDependencyModuleInformation(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(8, msg ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            9,
            Keywords = Keywords.RddEventKeywords,
            Message = "[msg=Runtime instrumentation agent is not attached. To enable runtime instrumentation agent monitoring, install Application Insights Agent.]",
            Level = EventLevel.Verbose)]
        public void RemoteDependencyModuleProfilerNotAttached(string appDomainName = "Incorrect")
        {
            this.WriteEvent(9, this.applicationNameProvider.Name);
        }

        [Event(
            10,
            Keywords = Keywords.RddEventKeywords,
            Message = "Begin callback called for id = '{0}', name= '{1}'",
            Level = EventLevel.Verbose)]
        public void BeginCallbackCalled(long id, string name, string appDomainName = "Incorrect")
        {
            this.WriteEvent(10, id, name ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            11,
            Keywords = Keywords.RddEventKeywords,
            Message = "End callback called for id = '{0}'",
            Level = EventLevel.Verbose)]
        public void EndCallbackCalled(string id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(11, id, this.applicationNameProvider.Name);
        }

        [Event(
            12,
            Keywords = Keywords.RddEventKeywords,
            Message = "End callback - cannot find start of operation for id = '{0}'",
            Level = EventLevel.Warning)]
        public void EndCallbackWithNoBegin(string id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(12, id, this.applicationNameProvider.Name);
        }

        [NonEvent]
        public void CallbackError(long id, string callbackName, Exception exception)
        {
            string idString = null;

            if (this.IsEnabled(EventLevel.Verbose, Keywords.RddEventKeywords))
            {
                idString = id.ToString(CultureInfo.InvariantCulture);
            }
            else if (this.IsEnabled(EventLevel.Error, Keywords.RddEventKeywords))
            {
                idString = "_";
            }
            else
            {
                return;
            }

            this.CallbackError(idString, callbackName ?? string.Empty, exception == null ? "null" : exception.ToInvariantString());
        }

        [Event(
            13,
            Keywords = Keywords.RddEventKeywords,
            Message = "Callback '{1}' failed for id = '{0}'. Exception: {2}",
            Level = EventLevel.Error)]
        public void CallbackError(string id, string callbackName, string exceptionString, string appDomainName = "Incorrect")
        {
            this.WriteEvent(13, id, callbackName ?? string.Empty, exceptionString ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            14,
            Keywords = Keywords.RddEventKeywords,
            Message = "Callback '{1}' will not run for id = '{0}'. Reason: {2}",
            Level = EventLevel.Warning)]
        public void NotExpectedCallback(long id, string callbackName, string reason, string appDomainName = "Incorrect")
        {
            this.WriteEvent(14, id, callbackName ?? string.Empty, reason ?? string.Empty, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs the information when the DependencyTelemetry item is null as warning.
        /// </summary>
        [Event(15, Message = "Dependency telemetry item is null.", Level = EventLevel.Warning)]
        public void DependencyTelemetryItemIsNullWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(15, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs the information when the HttpWebRequest is null as warning.
        /// </summary>
        [Event(16, Message = "WebRequest is null.", Level = EventLevel.Warning)]
        public void WebRequestIsNullWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(16, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs the information when a telemetry item that is already existing in the tables (that is currently being tracked) is tracked again.
        /// </summary>
        [Event(17, Message = "Tracking an already existing item in the table.", Level = EventLevel.Verbose)]
        public void TrackingAnExistingTelemetryItemVerbose(string appDomainName = "Incorrect")
        {
            this.WriteEvent(17, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs the information when the telemetry item to track is null.
        /// </summary>
        [Event(18, Message = "Telemetry to track is null.", Level = EventLevel.Warning)]
        public void TelemetryToTrackIsNullWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(18, this.applicationNameProvider.Name);
        }

        [Event(19, 
            Message = "RemoteDependency profiler failed to attach. Collection will default to EventSource implementation. Error details: {0}",
            Level = EventLevel.Error)]
        public void ProfilerFailedToAttachError(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(19, error ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            20,
            Keywords = Keywords.RddEventKeywords,
            Message = "UnexpectedCallbackParameter. Expected type: {0}.",
            Level = EventLevel.Warning)]
        public void UnexpectedCallbackParameter(string expectedType, string appDomainName = "Incorrect")
        {
            this.WriteEvent(20, expectedType ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            21,
            Keywords = Keywords.RddEventKeywords,
            Message = "End async callback called for id = '{0}'",
            Level = EventLevel.Verbose)]
        public void EndAsyncCallbackCalled(string id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(21, id, this.applicationNameProvider.Name);
        }

        [Event(
           22,
           Keywords = Keywords.RddEventKeywords,
           Message = "End async exception callback called for id = '{0}'",
           Level = EventLevel.Verbose)]
        public void EndAsyncExceptionCallbackCalled(string id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(22, id, this.applicationNameProvider.Name);
        }

        [Event(
            23,
            Message = "Current Activity is null for event = '{0}'",
            Level = EventLevel.Error)]
        public void CurrentActivityIsNull(string diagnosticsSourceEventName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(23, diagnosticsSourceEventName, this.applicationNameProvider.Name);
        }

        [Event(
            24,
            Message = "HttpDesktopDiagnosticSourceListener is activated.",
            Level = EventLevel.Verbose)]
        public void HttpDesktopDiagnosticSourceListenerIsActivated(string appDomainName = "Incorrect")
        {
            this.WriteEvent(24, this.applicationNameProvider.Name);
        }

        [Event(
            25,
            Message = "HttpDesktopDiagnosticSourceListener is deactivated.",
            Level = EventLevel.Verbose)]
        public void HttpDesktopDiagnosticSourceListenerIsDeactivated(string appDomainName = "Incorrect")
        {
            this.WriteEvent(25, this.applicationNameProvider.Name);
        }

        [Event(
            26,
            Message = "Telemetry for id = '{0}' is tracked with HttpDesktopDiagnosticSourceListener.",
            Level = EventLevel.Verbose)]
        public void SkipTrackingTelemetryItemWithEventSource(long id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(26, id, this.applicationNameProvider.Name);
        }

        [Event(
            27,
            Keywords = Keywords.RddEventKeywords,
            Message = "HttpDesktopDiagnosticSourceListener: Begin callback called for id = '{0}', name= '{1}'",
            Level = EventLevel.Verbose)]
        public void HttpDesktopBeginCallbackCalled(long id, string name, string appDomainName = "Incorrect")
        {
            this.WriteEvent(27, id, name ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            28,
            Keywords = Keywords.RddEventKeywords,
            Message = "HttpDesktopDiagnosticSourceListener: End callback called for id = '{0}'",
            Level = EventLevel.Verbose)]
        public void HttpDesktopEndCallbackCalled(long id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(28, id, this.applicationNameProvider.Name);
        }

        [Event(
            29,
            Keywords = Keywords.RddEventKeywords,
            Message = "System.Net.Http.HttpRequestOut.Start id = '{0}'",
            Level = EventLevel.Verbose)]
        public void HttpCoreDiagnosticSourceListenerStart(string id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(29, id, this.applicationNameProvider.Name);
        }

        [Event(
            30,
            Keywords = Keywords.RddEventKeywords,
            Message = "System.Net.Http.HttpRequestOut.Stop id = '{0}'",
            Level = EventLevel.Verbose)]
        public void HttpCoreDiagnosticSourceListenerStop(string id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(30, id, this.applicationNameProvider.Name);
        }

        [Event(
            31,
            Keywords = Keywords.RddEventKeywords,
            Message = "System.Net.Http.Request id = '{0}'",
            Level = EventLevel.Verbose)]
        public void HttpCoreDiagnosticSourceListenerRequest(Guid id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(31, id, this.applicationNameProvider.Name);
        }

        [Event(
            32,
            Keywords = Keywords.RddEventKeywords,
            Message = "System.Net.Http.Response id = '{0}'",
            Level = EventLevel.Verbose)]
        public void HttpCoreDiagnosticSourceListenerResponse(Guid id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(32, id, this.applicationNameProvider.Name);
        }

        [Event(
            33,
            Keywords = Keywords.RddEventKeywords,
            Message = "System.Net.Http.Exception id = '{0}'",
            Level = EventLevel.Verbose)]
        public void HttpCoreDiagnosticSourceListenerException(string id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(33, id, this.applicationNameProvider.Name);
        }

        [Event(
            34,
            Keywords = Keywords.RddEventKeywords,
            Message = "HttpCoreDiagnosticSubscriber failed to subscribe. Error details '{0}'",
            Level = EventLevel.Error)]
        public void HttpCoreDiagnosticSubscriberFailedToSubscribe(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(34, error, this.applicationNameProvider.Name);
        }

        [Event(
            35,
            Keywords = Keywords.RddEventKeywords,
            Message = "HttpDesktopDiagnosticSubscriber failed to subscribe. Error details '{0}'",
            Level = EventLevel.Error)]
        public void HttpDesktopDiagnosticSubscriberFailedToSubscribe(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(35, error, this.applicationNameProvider.Name);
        }

        [Event(
            36,
            Keywords = Keywords.RddEventKeywords,
            Message = "HttpHandlerDiagnosticListener failed to initialize. Error details '{0}'",
            Level = EventLevel.Error)]
        public void HttpHandlerDiagnosticListenerFailedToInitialize(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(36, error ?? string.Empty, this.applicationNameProvider.Name);
        }
        
        [Event(
            37,
            Keywords = Keywords.RddEventKeywords,
            Message = "HttpCoreDiagnosticSourceListener OnNext failed to call event handler. Error details '{0}'",
            Level = EventLevel.Error)]
        public void HttpCoreDiagnosticSourceListenerOnNextFailed(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(37, error, this.applicationNameProvider.Name);
        }

        [Event(
            38,
            Keywords = Keywords.RddEventKeywords,
            Message = "SqlClientDiagnosticSubscriber failed to subscribe. Error details '{0}'",
            Level = EventLevel.Error)]
        public void SqlClientDiagnosticSubscriberFailedToSubscribe(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(38, error, this.applicationNameProvider.Name);
        }

        [Event(
            39,
            Keywords = Keywords.RddEventKeywords,
            Message = "SqlClientDiagnosticSubscriber: Callback called for id = '{0}', name= '{1}'",
            Level = EventLevel.Verbose)]
        public void SqlClientDiagnosticSubscriberCallbackCalled(Guid id, string name, string appDomainName = "Incorrect")
        {
            this.WriteEvent(39, id, name ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            40,
            Keywords = Keywords.RddEventKeywords,
            Message = "SqlClientDiagnosticSourceListener OnNext failed to call event handler. Error details '{0}'",
            Level = EventLevel.Error)]
        public void SqlClientDiagnosticSourceListenerOnNextFailed(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(40, error, this.applicationNameProvider.Name);
        }

        [Event(
            41,
            Keywords = Keywords.RddEventKeywords,
            Message = "{0} failed to subscribe. Error details '{1}'",
            Level = EventLevel.Error)]
        public void DiagnosticSourceListenerFailedToSubscribe(string listenerName, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(41, listenerName, error, this.applicationNameProvider.Name);
        }

        [Event(
            42,
            Keywords = Keywords.RddEventKeywords,
            Message = "{0} id = '{1}'",
            Level = EventLevel.Verbose)]
        public void TelemetryDiagnosticSourceListenerEvent(string eventName, string id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(42, eventName, id, this.applicationNameProvider.Name);
        }

        [Event(
            43,
            Keywords = Keywords.RddEventKeywords,
            Message = "Failed to handle {0} event, error = '{1}' ",
            Level = EventLevel.Error)]
        public void TelemetryDiagnosticSourceCallbackException(string eventName, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(43, eventName, error, this.applicationNameProvider.Name);
        }

        [Event(
            44,
            Keywords = Keywords.RddEventKeywords,
            Message = "AutoTrackingDependencyTelemetry name {0}",
            Level = EventLevel.Verbose)]
        public void AutoTrackingDependencyItem(string depName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(44, depName, this.applicationNameProvider.Name);
        }

        [Event(
            45,
            Keywords = Keywords.RddEventKeywords,
            Message = "Ending operation for dependency name {0}, not tracking this item.",
            Level = EventLevel.Verbose)]
        public void EndOperationNoTracking(string depName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(45, depName, this.applicationNameProvider.Name);
        }

        [Event(
            46,
            Keywords = Keywords.RddEventKeywords,
            Message = "Not tracking operation for event = '{0}', id = '{1}', listener is not active.",
            Level = EventLevel.Verbose)]
        public void NotActiveListenerNoTracking(string evntName, string activityId, string appDomainName = "Incorrect")
        {
            this.WriteEvent(46, evntName, activityId, this.applicationNameProvider.Name);
        }

        [Event(
            47,
            Keywords = Keywords.RddEventKeywords,
            Message = "Detected Http Client instrumentation version {0} on for HttpClient version {1}.{2} with informational version {3}.",
            Level = EventLevel.Verbose)]
        public void HttpCoreDiagnosticListenerInstrumentationVersion(int httpInstrumentationVersion, int httpClientMajorVersion, int httpClientMinorVersion, string infoVersion, string appDomainName = "Incorrect")
        {
            this.WriteEvent(47, httpInstrumentationVersion, httpClientMajorVersion, httpClientMinorVersion, infoVersion, this.applicationNameProvider.Name);
        }

        [Event(
            48,
            Keywords = Keywords.RddEventKeywords,
            Message = "Http request is already instrumented.",
            Level = EventLevel.Verbose)]
        public void HttpRequestAlreadyInstrumented(string appDomainName = "Incorrect")
        {
            this.WriteEvent(48, this.applicationNameProvider.Name);
        }

        [Event(
            49,
            Keywords = Keywords.RddEventKeywords,
            Message = "Failed to parse Url '{0}'",
            Level = EventLevel.Warning)]
        public void FailedToParseUrl(string url, string appDomainName = "Incorrect")
        {
            this.WriteEvent(49, url, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Keywords for the <see cref="DependencyCollectorEventSource"/>.
        /// </summary>
        public sealed class Keywords
        {
            /// <summary>
            /// Key word for user actionable events.
            /// </summary>
            public const EventKeywords UserActionable = (EventKeywords)0x1;

            /*  Reserve first 3 for other service keywords
             *  public const EventKeywords Service1 = (EventKeywords)0x2;
             *  public const EventKeywords Service2 = (EventKeywords)0x4;
             *  public const EventKeywords Service3 = (EventKeywords)0x8;
             */

            /// <summary>
            /// Key word for resource discovery module failures.
            /// </summary>
            public const EventKeywords RddEventKeywords = (EventKeywords)0x400;
        }
    }
}