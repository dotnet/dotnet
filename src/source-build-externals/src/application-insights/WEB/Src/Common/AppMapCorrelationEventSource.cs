namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// ETW EventSource tracing class.
    /// </summary>
    //// [EventSource(Name = "Microsoft-ApplicationInsights-Extensibility-AppMapCorrelation")] - EVERY COMPONENT SHOULD DEFINE IT"S OWN NAME
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed partial class AppMapCorrelationEventSource : EventSource
    {
        public static readonly AppMapCorrelationEventSource Log = new AppMapCorrelationEventSource();
        private readonly ApplicationNameProvider applicationNameProvider = new ApplicationNameProvider();

        private AppMapCorrelationEventSource()
        {
        }

        [Event(
            1,
            Keywords = Keywords.UserActionable,
            Message = "Failed to retrieve App ID for the current application insights resource. Make sure the configured instrumentation key is valid. Error: {0}",
            Level = EventLevel.Warning)]
        public void FetchAppIdFailed(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(1, exception, this.applicationNameProvider.Name);
        }

        [Event(
            2,
            Keywords = Keywords.Diagnostics,
            Message = "Failed to add cross component correlation header. Error: {0}",
            Level = EventLevel.Warning)]
        public void SetCrossComponentCorrelationHeaderFailed(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(2, exception, this.applicationNameProvider.Name);
        }

        [Event(
            3,
            Keywords = Keywords.Diagnostics,
            Message = "Failed to determine cross component correlation header. Error: {0}",
            Level = EventLevel.Warning)]
        public void GetCrossComponentCorrelationHeaderFailed(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(3, exception, this.applicationNameProvider.Name);
        }

        [Event(
            4,
            Keywords = Keywords.Diagnostics,
            Message = "Failed to determine role name header. Error: {0}",
            Level = EventLevel.Warning)]
        public void GetComponentRoleNameHeaderFailed(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(4, exception, this.applicationNameProvider.Name);
        }

        [Event(
            5,
            Keywords = Keywords.Diagnostics,
            Message = "Unknown error occurred.",
            Level = EventLevel.Warning)]
        public void UnknownError(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(5, exception, this.applicationNameProvider.Name);
        }

        [Event(
            6,
            Keywords = Keywords.UserActionable,
            Message = "Failed to retrieve App ID for the current application insights resource. Endpoint returned HttpStatusCode: {0}",
            Level = EventLevel.Warning)]
        public void FetchAppIdFailedWithResponseCode(string httpStatusCode, string appDomainName = "Incorrect")
        {
            this.WriteEvent(6, httpStatusCode, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Keywords for the <see cref="AppMapCorrelationEventSource"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Required By ETW manifest")]
        public sealed class Keywords
        {
            /// <summary>
            /// Key word for user actionable events.
            /// </summary>
            public const EventKeywords UserActionable = (EventKeywords)0x1;

            /// <summary>
            /// Key word for diagnostics events.
            /// </summary>
            public const EventKeywords Diagnostics = (EventKeywords)0x2;
        }
    }
}