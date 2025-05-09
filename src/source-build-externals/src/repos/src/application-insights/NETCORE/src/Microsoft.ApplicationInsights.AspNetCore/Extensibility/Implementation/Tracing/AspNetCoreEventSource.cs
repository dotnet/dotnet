namespace Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing
{
#if AI_ASPNETCORE_WEB
    using Microsoft.ApplicationInsights.AspNetCore.Implementation;
    using Microsoft.ApplicationInsights.Shared.Internals;
#endif
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;

    /// <summary>
    /// Event source for Application Insights ASP.NET Core SDK.
    /// </summary>
#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-AspNetCore")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-AspNetCore")]
#endif
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    [SuppressMessage("", "SA1611:ElementParametersMustBeDocumented", Justification = "Internal only class.")]
    internal sealed class AspNetCoreEventSource : EventSource
    {
        /// <summary>
        /// The singleton instance of this event source.
        /// Due to how EventSource initialization works this has to be a public field and not
        /// a property otherwise the internal state of the event source will not be enabled.
        /// </summary>
        public static readonly AspNetCoreEventSource Instance = new AspNetCoreEventSource();
        private readonly ApplicationNameProvider applicationNameProvider = new ApplicationNameProvider();

        /// <summary>
        /// Prevents a default instance of the <see cref="AspNetCoreEventSource"/> class from being created.
        /// </summary>
        private AspNetCoreEventSource()
            : base()
        {
        }

        /// <summary>
        /// Logs an event for the TelemetryInitializerBase Initialize method when the HttpContext is null.
        /// </summary>
        /// <param name="appDomainName">An ignored placeholder to make EventSource happy.</param>
        [Event(2, Message = "TelemetryInitializerBase.Initialize - httpContextAccessor.HttpContext is null, returning.", Level = EventLevel.Warning, Keywords = Keywords.Diagnostics)]
        public void LogTelemetryInitializerBaseInitializeContextNull(string appDomainName = "Incorrect")
        {
            this.WriteEvent(2, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event for the TelemetryInitializerBase Initialize method when the request is null.
        /// </summary>
        /// <param name="appDomainName">An ignored placeholder to make EventSource happy.</param>
        [Event(4, Message = "TelemetryInitializerBase.Initialize - request is null, returning.", Level = EventLevel.Warning, Keywords = Keywords.Diagnostics)]
        public void LogTelemetryInitializerBaseInitializeRequestNull(string appDomainName = "Incorrect")
        {
            this.WriteEvent(4, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event for the ClientIpHeaderTelemetryInitializer OnInitializeTelemetry method when the location IP is null.
        /// </summary>
        /// <param name="appDomainName">An ignored placeholder to make EventSource happy.</param>
        [Event(5, Message = "ClientIpHeaderTelemetryInitializer.OnInitializeTelemetry - telemetry.Context.Location.Ip is already set, returning.", Level = EventLevel.Warning, Keywords = Keywords.Diagnostics)]
        public void LogClientIpHeaderTelemetryInitializerOnInitializeTelemetryIpNull(string appDomainName = "Incorrect")
        {
            this.WriteEvent(5, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event for the WebSessionTelemetryInitializer OnInitializeTelemetry method when the session Id is null.
        /// </summary>
        /// <param name="appDomainName">An ignored placeholder to make EventSource happy.</param>
        [Event(6, Message = "WebSessionTelemetryInitializer.OnInitializeTelemetry - telemetry.Context.Session.Id is null or empty, returning.", Level = EventLevel.Warning, Keywords = Keywords.Diagnostics)]
        public void LogWebSessionTelemetryInitializerOnInitializeTelemetrySessionIdNull(string appDomainName = "Incorrect")
        {
            this.WriteEvent(6, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event for the WebUserTelemetryInitializer OnInitializeTelemetry method when the session Id is null.
        /// </summary>
        /// <param name="appDomainName">An ignored placeholder to make EventSource happy.</param>
        [Event(7, Message = "WebUserTelemetryInitializer.OnInitializeTelemetry - telemetry.Context.Session.Id is null or empty, returning.", Level = EventLevel.Warning, Keywords = Keywords.Diagnostics)]
        public void LogWebUserTelemetryInitializerOnInitializeTelemetrySessionIdNull(string appDomainName = "Incorrect")
        {
            this.WriteEvent(7, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event for the HostingDiagnosticListener OnHttpRequestInStart method when the current activity is null.
        /// </summary>
        /// <param name="appDomainName">An ignored placeholder to make EventSource happy.</param>
        [Event(9, Message = "HostingDiagnosticListener.OnHttpRequestInStart - Activity.Current is null, returning.", Level = EventLevel.Warning, Keywords = Keywords.Diagnostics)]
        public void LogHostingDiagnosticListenerOnHttpRequestInStartActivityNull(string appDomainName = "Incorrect")
        {
            this.WriteEvent(9, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event when a TelemetryModule is not found to configure.
        /// </summary>
        [Event(11, Message = "Unable to configure module {0} as it is not found in service collection.", Level = EventLevel.Error, Keywords = Keywords.Diagnostics)]
        public void UnableToFindModuleToConfigure(string moduleType, string appDomainName = "Incorrect")
        {
            this.WriteEvent(11, moduleType, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event when telemetry is not tracked as the Listener is not active.
        /// </summary>
        [Event(
            13,
            Keywords = Keywords.Diagnostics,
            Message = "Not tracking operation for event = '{0}', id = '{1}', lisener is not active.",
            Level = EventLevel.Verbose)]
        public void NotActiveListenerNoTracking(string evntName, string activityId, string appDomainName = "Incorrect")
        {
            this.WriteEvent(13, evntName, activityId, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event for when generic error occur within the SDK.
        /// </summary>
        [Event(
            14,
            Keywords = Keywords.Diagnostics,
            Message = "An error has occurred which may prevent application insights from functioning. Error message: '{0}' ",
            Level = EventLevel.Error)]
        public void LogError(string errorMessage, string appDomainName = "Incorrect")
        {
            this.WriteEvent(14, errorMessage, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event when RequestTrackingModule failed to initialize.
        /// </summary>
        [Event(
            15,
            Keywords = Keywords.Diagnostics,
            Message = "An error has occurred while initializing RequestTrackingModule. Requests will not be auto collected. Error message: '{0}' ",
            Level = EventLevel.Error)]
        public void RequestTrackingModuleInitializationFailed(string errorMessage, string appDomainName = "Incorrect")
        {
            this.WriteEvent(15, errorMessage, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event when any error occurs within DiagnosticListener callback.
        /// </summary>
        [Event(
            16,
            Keywords = Keywords.Diagnostics,
            Message = "An error has occurred in DiagnosticSource listener. Callback: '{0}'. Error message: '{1}' ",
            Level = EventLevel.Warning)]
        public void DiagnosticListenerWarning(string callback, string errorMessage, string appDomainName = "Incorrect")
        {
            this.WriteEvent(16, callback, errorMessage, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event when TelemetryConfiguration configure has failed.
        /// </summary>
        [Event(
            17,
            Keywords = Keywords.Diagnostics,
            Message = "An error has occurred while setting up TelemetryConfiguration. Error message: '{0}' ",
            Level = EventLevel.Error)]
        public void TelemetryConfigurationSetupFailure(string errorMessage, string appDomainName = "Incorrect")
        {
            this.WriteEvent(17, errorMessage, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event when a telemetry item is sampled out at head.
        /// </summary>
        [Event(
            18,
            Keywords = Keywords.Diagnostics,
            Message = "Telemetry item was sampled out at head, OperationId: '{0}'",
            Level = EventLevel.Verbose)]
        public void TelemetryItemWasSampledOutAtHead(string operationId, string appDomainName = "Incorrect")
        {
            this.WriteEvent(18, operationId, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an informational event from Hosting listeners.
        /// </summary>
        [Event(
            19,
            Message = "Hosting Major Version: '{0}'. Informational Message: '{1}'.",
            Level = EventLevel.Informational)]
        public void HostingListenerInformational(AspNetCoreMajorVersion hostingVersion, string message, string appDomainName = "Incorrect")
        {
            this.WriteEvent(19, hostingVersion, message, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs a verbose event.
        /// </summary>
        [Event(
            20,
            Message = "Message: '{0}'.",
            Level = EventLevel.Verbose)]
        public void HostingListenerVerbose(string message, string appDomainName = "Incorrect")
        {
            this.WriteEvent(20, message, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event for RequestTelemetry created.
        /// </summary>
        [Event(
            21,
            Message = "RequestTelemetry created. CorrelationFormat: '{0}', RequestID: '{1}', OperationId : '{2}' ",
            Level = EventLevel.Informational)]
        public void RequestTelemetryCreated(string correlationFormat, string requestId, string requestOperationId, string appDomainName = "Incorrect")
        {
            this.WriteEvent(21, correlationFormat, requestId, requestOperationId, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs a verbose event.
        /// </summary>
        [Event(
            22,
            Message = "Message: '{0}'. Exception: '{1}'",
            Level = EventLevel.Warning)]
        public void HostingListenerWarning(string message, string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(22, message, exception, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an informational event.
        /// </summary>
        [Event(
            23,
            Message = "Message : {0}",
            Level = EventLevel.Informational)]
        public void LogInformational(string message, string appDomainName = "Incorrect")
        {
            this.WriteEvent(23, message, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event when AzureAppServiceRoleNameFromHostNameHeaderInitializer faces errors.
        /// </summary>
        /// <param name="exception">Exception message.</param>
        /// <param name="appDomainName">An ignored placeholder to make EventSource happy.</param>
        [Event(24, Message = "An error has occurred in AzureAppServiceRoleNameFromHostNameHeaderInitializer. Exception: '{0}'", Level = EventLevel.Warning, Keywords = Keywords.Diagnostics)]
        public void LogAzureAppServiceRoleNameFromHostNameHeaderInitializerWarning(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(24, exception, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs an event when TelemetryModule initialization has failed.
        /// </summary>
        [Event(
            25,
            Keywords = Keywords.Diagnostics,
            Message = "An error has occurred while initializing the TelemetryModule: '{0}'. Error message: '{1}' ",
            Level = EventLevel.Error)]
        public void TelemetryModuleInitialziationSetupFailure(string moduleName, string errorMessage, string appDomainName = "Incorrect")
        {
            this.WriteEvent(25, moduleName, errorMessage, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Keywords for the AspNetEventSource.
        /// </summary>
        public sealed class Keywords
        {
            /// <summary>
            /// Keyword for errors that trace at Verbose level.
            /// </summary>
            public const EventKeywords Diagnostics = (EventKeywords)0x1;
        }
    }
}
