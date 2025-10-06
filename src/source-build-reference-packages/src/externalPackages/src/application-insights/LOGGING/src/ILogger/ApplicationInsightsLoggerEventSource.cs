// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsLoggerEventSource.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Extensions.Logging.ApplicationInsights
{
    using System.Diagnostics.Tracing;
    using System.Reflection;

    /// <summary>
    /// EventSource for reporting errors and warnings from Logging module.
    /// </summary>
#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-LoggerProvider")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-LoggerProvider")]
#endif
    internal sealed class ApplicationInsightsLoggerEventSource : EventSource
    {
        public static readonly ApplicationInsightsLoggerEventSource Log = new ApplicationInsightsLoggerEventSource();
        public readonly string ApplicationName;

        private ApplicationInsightsLoggerEventSource()
        {
            this.ApplicationName = GetApplicationName();
        }

        [Event(1, Message = "Sending log to ApplicationInsightsLoggerProvider has failed. Error: {0}", Level = EventLevel.Error)]
        public void FailedToLog(string error, string applicationName = null) => this.WriteEvent(1, error, applicationName ?? this.ApplicationName);

        [NonEvent]
        private static string GetApplicationName()
        {
            try
            {
                return Assembly.GetEntryAssembly().GetName().Name;
            }
            catch
            {
                return "Unknown";
            }
        }
    }
}
