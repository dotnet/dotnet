namespace Microsoft.ApplicationInsights.Web.Implementation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
#if NET452
    using System.Diagnostics.Tracing;
#endif
    using Microsoft.ApplicationInsights.Common;

    /// <summary>
    /// ETW EventSource tracing class.
    /// </summary>
#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-Extensibility-Web")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-Extensibility-Web")]
#endif
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed class WebEventSource : EventSource
    {
        /// <summary>
        /// Instance of the PlatformEventSource class.
        /// </summary>
        public static readonly WebEventSource Log = new WebEventSource();
        private readonly ApplicationNameProvider applicationNameProvider = new ApplicationNameProvider();

        private WebEventSource()
        {
        }

        public static bool IsVerboseEnabled 
        { 
             [NonEvent] 
             get 
             { 
                 return Log.IsEnabled(EventLevel.Verbose, (EventKeywords)(-1)); 
             } 
         }

        [Event(
            1,
            Message = "ApplicationInsightsHttpModule failed at initialization with exception: {0}",
            Level = EventLevel.Error)]
        public void WebModuleInitializationExceptionEvent(string excMessage, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                1,
                excMessage ?? string.Empty,
                this.applicationNameProvider.Name);
        }

        [Event(
            2,
            Message = "ApplicationInsightsHttpModule failed at {0} with exception: {1}",
            Level = EventLevel.Warning)]
        public void TraceCallbackFailure(string callbackName, string excMessage, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                2,
                callbackName ?? string.Empty,
                excMessage ?? string.Empty,
                this.applicationNameProvider.Name);
        }

        [Event(
            3,
            Message = "[msg=WebModuleCallback];[callback={0}];[uri={1}];",
            Level = EventLevel.Verbose)]
        public void WebModuleCallback(string callback, string uri, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                3,
                callback ?? string.Empty,
                uri ?? string.Empty,
                this.applicationNameProvider.Name);
        }

        [Event(
            4,
            Message = "[msg=HanderFailure];[exception={0}];",
            Level = EventLevel.Error)]
        public void HanderFailure(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                4,
                exception ?? string.Empty,
                this.applicationNameProvider.Name);
        }

        [Event(
            5,
            Message = "[msg=UserHostNotCollectedWarning];[exception={0}];",
            Level = EventLevel.Warning)]
        public void UserHostNotCollectedWarning(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                5,
                exception ?? string.Empty,
                this.applicationNameProvider.Name);
        }

        [Event(
            8,
            Message = "[msg=RequestTelemetryCreated];",
            Level = EventLevel.Verbose)]
        public void WebTelemetryModuleRequestTelemetryCreated(string appDomainName = "Incorrect")
        {
            this.WriteEvent(8, this.applicationNameProvider.Name);
        }

        [Event(
            9,
            Message = "[msg=HttpRequestNotAvailable];[msg={0}];[stack={1}];",
            Level = EventLevel.Warning)]
        public void HttpRequestNotAvailable(string message, string stack, string appDomainName = "Incorrect")
        {
            this.WriteEvent(9, message, stack, this.applicationNameProvider.Name);
        }

        [Event(
            10,
            Keywords = Keywords.VerboseFailure,
            Message = "[msg=WebSessionTrackingSessionCookieIsNotSpecifiedInRequest];",
            Level = EventLevel.Verbose)]
        public void WebSessionTrackingSessionCookieIsNotSpecifiedInRequest(string appDomainName = "Incorrect")
        {
            this.WriteEvent(10, this.applicationNameProvider.Name);
        }

        [Event(
            11,
            Message = "[msg=WebSessionTrackingSessionCookieIsEmptyWarning];",
            Level = EventLevel.Warning)]
        public void WebSessionTrackingSessionCookieIsEmptyWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(11, this.applicationNameProvider.Name);
        }

        [Event(
            12,
            Message = "[msg=WebUserTrackingUserCookieNotAvailable];",
            Level = EventLevel.Verbose)]
        public void WebUserTrackingUserCookieNotAvailable(string appDomainName = "Incorrect")
        {
            this.WriteEvent(12, this.applicationNameProvider.Name);
        }

        [Event(
            13,
            Message = "[msg=WebUserTrackingUserCookieIsEmpty];",
            Level = EventLevel.Warning)]
        public void WebUserTrackingUserCookieIsEmpty(string appDomainName = "Incorrect")
        {
            this.WriteEvent(13, this.applicationNameProvider.Name);
        }

        [Event(
            14,
            Message = "[msg=WebUserTrackingUserCookieIsIncomplete];[cookieValue={0}];",
            Level = EventLevel.Warning)]
        public void WebUserTrackingUserCookieIsIncomplete(
            string cookieValue, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                14,
                cookieValue ?? string.Empty,
                this.applicationNameProvider.Name);
        }

        [Event(
            16,
            Message = "WebTelemetryInitializerLoaded at {0}",
            Level = EventLevel.Verbose)]
        public void WebTelemetryInitializerLoaded(
            string typeName,
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                16,
                typeName ?? string.Empty,
                this.applicationNameProvider.Name);
        }

        [Event(
            17,
            Message = "[msg=WebTelemetryInitializerNotExecutedOnNullHttpContext]",
            Level = EventLevel.Verbose)]
        public void WebTelemetryInitializerNotExecutedOnNullHttpContext(
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(17, this.applicationNameProvider.Name);
        }

        [Event(
            18,
            Message = "TelemetryInitializer {0} failed to initialize telemetry item {1}",
            Level = EventLevel.Error)]
        public void WebTelemetryInitializerFailure(
            string typeName,
            string exception,
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                18,
                typeName ?? string.Empty,
                exception ?? string.Empty,
                this.applicationNameProvider.Name);
        }

        [Event(
            19,
            Message = "[msg=WebSetLocationIdSkipped];[headerName={0}];",
            Level = EventLevel.Verbose)]
        public void WebLocationIdHeaderFound(string headerName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(19, headerName ?? "NULL", this.applicationNameProvider.Name);
        }

        [Event(
            20,
            Message = "[msg=WebSetLocationIdSet];[ip={0}];",
            Level = EventLevel.Verbose)]
        public void WebLocationIdSet(string ip, string appDomainName = "Incorrect")
        {
            this.WriteEvent(20, ip ?? "NULL", this.applicationNameProvider.Name);
        }

        [Event(
            21,
            Message = "[msg=WebUriFormatException];",
            Level = EventLevel.Warning)]
        public void WebUriFormatException(string appDomainName = "Incorrect")
        {
            this.WriteEvent(21, this.applicationNameProvider.Name);
        }

        [Event(
            26,
            Message = "[msg=AuthIdTrackingCookieNotAvailable];",
            Level = EventLevel.Verbose)]
        public void AuthIdTrackingCookieNotAvailable(string appDomainName = "Incorrect")
        {
            this.WriteEvent(26, this.applicationNameProvider.Name);
        }

        [Event(
            27,
            Message = "[msg=AuthIdTrackingCookieIsEmpty];",
            Level = EventLevel.Warning)]
        public void AuthIdTrackingCookieIsEmpty(string appDomainName = "Incorrect")
        {
            this.WriteEvent(27, this.applicationNameProvider.Name);
        }

        [Event(
            28,
            Message = "[msg=ThreadAbortWarning];",
            Level = EventLevel.Warning)]
        public void ThreadAbortWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                28,
                this.applicationNameProvider.Name);
        }

        [Event(
            29,
            Message = "[msg=WebRequestFilteredOutByUserAgent];",
            Level = EventLevel.Verbose)]
        public void WebRequestFilteredOutByUserAgent(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                29,
                this.applicationNameProvider.Name);
        }

        [Event(
            30,
            Message = "[msg=WebRequestFilteredOutByRequestHandler];",
            Level = EventLevel.Verbose)]
        public void WebRequestFilteredOutByRequestHandler(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                30,
                this.applicationNameProvider.Name);
        }

        [Event(
            31,
            Keywords = Keywords.UserActionable,
            Message = "SyntheticUserAgentTelemetryInitializer failed to parse regular expression {0} with exception: {1}",
            Level = EventLevel.Warning)]
        public void SyntheticUserAgentTelemetryInitializerRegularExpressionParsingException(string pattern, string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                31,
                pattern,
                exception,
                this.applicationNameProvider.Name);
        }

        [Event(
            32,
            Message = "FlagCheckFailure {0}.",
            Level = EventLevel.Error)]
        public void FlagCheckFailure(string excMessage, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                32,
                excMessage ?? string.Empty,
                this.applicationNameProvider.Name);
        }

        [Event(
            33,
            Message = "RequestFiltered",
            Level = EventLevel.Verbose)]
        public void RequestFiltered(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                33,
                this.applicationNameProvider.Name);
        }

        [Event(
            34,
            Message = "[msg=NoHttpContext];",
            Level = EventLevel.Warning)]
        public void NoHttpContextWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                34,
                this.applicationNameProvider.Name);
        }

        [Event(
            35,
            Message = "Failed to hook onto AddOnSendingHeaders event. Exception {0}",
            Level = EventLevel.Warning)]
        public void HookAddOnSendingHeadersFailedWarning(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                35,
                exception,
                this.applicationNameProvider.Name);
        }

        [Event(
            36,
            Message = "Failed to add target instrumentation key hash as a response header. Exception {0}",
            Level = EventLevel.Warning)]
        public void AddTargetHeaderFailedWarning(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                36,
                exception,
                this.applicationNameProvider.Name);
        }

        [Event(
            37,
            Message = "Initialize has not been called on this module yet.",
            Level = EventLevel.Error)]
        public void InitializeHasNotBeenCalledOnModuleYetError(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                37,
                this.applicationNameProvider.Name);
        }

        [Event(
            38,
            Message = "Initialize has not been called on this module yet.",
            Level = EventLevel.Verbose)]
        public void InitializeHasNotBeenCalledOnModuleYetVerbose(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                38,
                this.applicationNameProvider.Name);
        }

        [Event(
            39,
            Message = "RequestTrackingTelemetryModule ChildRequestTrackingSuppressionModule Method: '{0}' Unknown Exception: {1}",
            Level = EventLevel.Error)]
        public void ChildRequestUnknownException(string methodName, string exceptionMessage, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                39,
                methodName,
                exceptionMessage,
                this.applicationNameProvider.Name);
        }

        [Event(
            40,
            Message = "RequestTrackingTelemetryModule: Request was not logged. Set EventLevel Verbose for more details.",
            Level = EventLevel.Informational)]
        public void RequestTrackingTelemetryModuleRequestWasNotLoggedInformational(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                40,
                this.applicationNameProvider.Name);
        }

        [Event(
            41,
            Message = "RequestTrackingTelemetryModule: Request was not logged. Request Id: '{0}' Reason: {1}",
            Level = EventLevel.Verbose)]
        public void RequestTrackingTelemetryModuleRequestWasNotLoggedVerbose(string requestId, string reason, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                41,
                requestId,
                reason,
                this.applicationNameProvider.Name);
        }

        [Event(42,
            Keywords = Keywords.Diagnostics,
            Message = "Injection started.",
            Level = EventLevel.Verbose)]
        public void InjectionStarted(string appDomainName = "Incorrect")
        {
            this.WriteEvent(42, this.applicationNameProvider.Name);
        }

        [Event(43,
            Keywords = Keywords.Diagnostics,
            Message = "{0} Injection failed. Error message: {1}",
            Level = EventLevel.Informational)]
        public void InjectionFailed(string component, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(43, component ?? string.Empty, error ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(44,
            Keywords = Keywords.Diagnostics,
            Message = "Version '{0}' of component '{1}' is not supported",
            Level = EventLevel.Verbose)]
        public void InjectionVersionNotSupported(string version, string component, string appDomainName = "Incorrect")
        {
            this.WriteEvent(44, version ?? string.Empty, component ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            45,
            Keywords = Keywords.Diagnostics,
            Message = "Unknown exception. Error message: {0}.",
            Level = EventLevel.Error)]
        public void InjectionUnknownError(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(45, error ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            46,
            Keywords = Keywords.Diagnostics,
            Message = "Another exception filter or logger is already injected. Type: '{0}', component: '{1}'",
            Level = EventLevel.Verbose)]
        public void InjectionSkipped(string type, string component, string appDomainName = "Incorrect")
        {
            this.WriteEvent(46, type ?? string.Empty, component ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(47,
            Keywords = Keywords.Diagnostics,
            Message = "Injection completed.",
            Level = EventLevel.Verbose)]
        public void InjectionCompleted(string appDomainName = "Incorrect")
        {
            this.WriteEvent(47, this.applicationNameProvider.Name);
        }

        [Event(48,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable,
            Message = "Microsoft.AspNet.TelemetryCorrelation.TelemetryCorrelationHttpModule is not first in the HTTP modules list.",
            Level = EventLevel.Error)]
        public void RequestTelemetryCreatedBeforeTelemetryCorrelationModuleRuns(string appDomainName = "Incorrect")
        {
            this.WriteEvent(48, this.applicationNameProvider.Name);
        }

        [Event(49,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable,
            Message = ".NET 4.7.1 is not installed, correlation for HTTP requests with body is not possible",
            Level = EventLevel.Error)]
        public void CorrelationIssueIsDetectedForRequestWithBody(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                49,
                this.applicationNameProvider.Name);
        }

        [Event(50,
            Keywords = Keywords.Diagnostics,
            Message = "Unknown error, message {0}",
            Level = EventLevel.Error)]
        public void UnknownError(string message, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                50,
                message,
                this.applicationNameProvider.Name);
        }

        [Event(51,
            Keywords = Keywords.Diagnostics,
            Message = "An error has occurred in AzureAppServiceRoleNameFromHostNameHeaderInitializer. Exception: '{0}'",
            Level = EventLevel.Warning)]
        public void LogAzureAppServiceRoleNameFromHostNameHeaderInitializerWarning(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                51,
                exception,
                this.applicationNameProvider.Name);
        }

        [Event(52, Message = "Failed to set RequestTelemetry URL. RawUrl: '{0}' Exception: '{1}'", Level = EventLevel.Warning)]
        public void FailedToSetRequestTelemetryUrl(string rawUrl, string exception, string appDomainName = "Incorrect") => this.WriteEvent(52, rawUrl, exception, this.applicationNameProvider.Name);

        [Event(53,
            Keywords = Keywords.Diagnostics,
            Message = "Failed to invoke OnExecuteRequestStep, Error='{0}'", 
            Level = EventLevel.Warning)]
        public void OnExecuteRequestStepInvocationError(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(53, error, this.applicationNameProvider.Name);
        }

        [Event(
            54,
            Message = "Http application is null",
            Level = EventLevel.Warning)]
        public void NoHttpApplicationWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                54,
                this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Keywords for the PlatformEventSource. Those keywords should match keywords in Core.
        /// </summary>
        public sealed class Keywords
        {
            /// <summary>
            /// Key word for user actionable events.
            /// </summary>
            public const EventKeywords UserActionable = (EventKeywords)0x1;

            /// <summary>
            /// Diagnostics tracing keyword.
            /// </summary>
            public const EventKeywords Diagnostics = (EventKeywords)0x2;

            /// <summary>
            /// Keyword for errors that trace at Verbose level.
            /// </summary>
            public const EventKeywords VerboseFailure = (EventKeywords)0x4;
        }
    }
}
