//-----------------------------------------------------------------------
// <copyright file="EventSourceListenerEventSource.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.TraceEvent.Shared.Implementation
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Reflection;

    /// <summary>
    /// EventSource for reporting errors and warnings from the EventSourceListener telemetry module.
    /// </summary>
    [EventSource(Name = ProviderName)]
    internal sealed class EventSourceListenerEventSource : EventSource
    {
#if REDFIELD
        public const string ProviderName = "Redfield-Microsoft-ApplicationInsights-Extensibility-EventSourceListener";
#else
        public const string ProviderName = "Microsoft-ApplicationInsights-Extensibility-EventSourceListener";
#endif
        public static readonly EventSourceListenerEventSource Log = new EventSourceListenerEventSource();

        public readonly string ApplicationName;

        private const int NoEventSourcesConfiguredEventId = 1;
        private const int FailedToEnableProvidersEventId = 2;
        private const int ModuleInitializationFailedEventId = 3;
        private const int UnauthorizedAccessEventId = 4;
        private const int OnEventWrittenHandlerFailure = 5;

        private EventSourceListenerEventSource()
        {
            this.ApplicationName = GetApplicationName();
        }

        [Event(NoEventSourcesConfiguredEventId, Level = EventLevel.Warning, Keywords = Keywords.Configuration, Message = "No Sources configured for the {1}")]
        public void NoSourcesConfigured(string moduleName, string applicationName = null)
        {
            this.WriteEvent(NoEventSourcesConfiguredEventId, applicationName ?? this.ApplicationName, moduleName);
        }

        [Event(FailedToEnableProvidersEventId, Level = EventLevel.Error, Keywords = Keywords.Configuration, Message = "Failed to enable provider {1} for the {0}.")]
        public void FailedToEnableProviders(string moduleName, string providerName, string details, string applicationName = null)
        {
            this.WriteEvent(FailedToEnableProvidersEventId, moduleName, providerName, details, applicationName ?? this.ApplicationName);
        }

        [Event(ModuleInitializationFailedEventId, Level = EventLevel.Error, Keywords = Keywords.Configuration, Message = "Initialization failed for the {0}.")]
        public void ModuleInitializationFailed(string moduleName, string details, string applicationName = null)
        {
            this.WriteEvent(ModuleInitializationFailedEventId, moduleName, details, applicationName ?? this.ApplicationName);
        }

        [Event(UnauthorizedAccessEventId, Level = EventLevel.Error, Keywords = Keywords.Configuration, Message = "Failed to enable provider for the {0}. Access Denied.")]
        public void AccessDenied(string moduleName, string details, string applicationName = null)
        {
            this.WriteEvent(UnauthorizedAccessEventId, moduleName, details, applicationName ?? this.ApplicationName);
        }

        [Event(OnEventWrittenHandlerFailure, Level = EventLevel.Error, Message = "{0}: Failure while handling event")]
        public void OnEventWrittenHandlerFailed(string moduleName, string details, string applicationName = null)
        {
            this.WriteEvent(OnEventWrittenHandlerFailure, moduleName, details, applicationName ?? this.ApplicationName);
        }

        [NonEvent]
        private static string GetApplicationName()
        {
            string name;
            try
            {
#if NET452 || NET46
                name = AppDomain.CurrentDomain.FriendlyName;
#else
                name = string.Empty;
#endif
            }
            catch
            {
                name = "(unknown)";
            }

            return name;
        }

        public sealed class Keywords
        {
            public const EventKeywords Configuration = (EventKeywords)0x01;
        }
    }
}
