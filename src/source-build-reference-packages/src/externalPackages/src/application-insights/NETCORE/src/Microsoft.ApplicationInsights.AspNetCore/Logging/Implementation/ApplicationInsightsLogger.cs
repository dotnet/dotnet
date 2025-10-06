namespace Microsoft.ApplicationInsights.AspNetCore.Logging
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.Extensions.Logging;

#pragma warning disable CS0618 // ApplicationInsightsLoggerOptions is obsolete. This will not be fixed because this class is also obsolete.
    /// <summary>
    /// <see cref="ILogger"/> implementation that forwards log messages as Application Insight trace events.
    /// </summary>
    [SuppressMessage("Documentation Rules", "SA1614:ElementParameterDocumentationMustHaveText", Justification = "This class is obsolete and will not be completely documented.")]
    internal class ApplicationInsightsLogger : ILogger
    {
#if NETFRAMEWORK
        /// <summary>
        /// SDK Version Prefix.
        /// </summary>
        public const string VersionPrefix = "ilf:";
#else
        /// <summary>
        /// SDK Version Prefix.
        /// </summary>
        public const string VersionPrefix = "ilc:";
#endif

        private readonly string categoryName;
        private readonly TelemetryClient telemetryClient;
        private readonly Func<string, LogLevel, bool> filter;
        private readonly ApplicationInsightsLoggerOptions options;
        private readonly string sdkVersion = SdkVersionUtils.GetVersion(VersionPrefix);

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInsightsLogger"/> class.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="name"></param>
        /// <param name="options"></param>
        /// <param name="telemetryClient"></param>
        public ApplicationInsightsLogger(string name, TelemetryClient telemetryClient, Func<string, LogLevel, bool> filter, ApplicationInsightsLoggerOptions options)
        {
            this.categoryName = name;
            this.telemetryClient = telemetryClient;
            this.filter = filter;
            this.options = options;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return this.filter != null && this.telemetryClient != null && this.filter(this.categoryName, logLevel) && this.telemetryClient.IsEnabled();
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (this.IsEnabled(logLevel))
            {
                var stateDictionary = state as IReadOnlyList<KeyValuePair<string, object>>;
                if (exception == null || this.options?.TrackExceptionsAsExceptionTelemetry == false)
                {
                    var traceTelemetry = new TraceTelemetry(formatter(state, exception), GetSeverityLevel(logLevel));
                    this.PopulateTelemetry(traceTelemetry, stateDictionary, eventId);
                    this.telemetryClient.TrackTrace(traceTelemetry);
                }
                else
                {
                    var exceptionTelemetry = new ExceptionTelemetry(exception);
                    exceptionTelemetry.Message = formatter(state, exception);
                    exceptionTelemetry.SeverityLevel = GetSeverityLevel(logLevel);
                    exceptionTelemetry.Properties["Exception"] = exception.ToString();
                    exception.Data.Cast<DictionaryEntry>().ToList().ForEach((item) => exceptionTelemetry.Properties[item.Key.ToString()] = (item.Value ?? "null").ToString());
                    this.PopulateTelemetry(exceptionTelemetry, stateDictionary, eventId);
                    this.telemetryClient.TrackException(exceptionTelemetry);
                }
            }
        }

        private static SeverityLevel GetSeverityLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return SeverityLevel.Critical;
                case LogLevel.Error:
                    return SeverityLevel.Error;
                case LogLevel.Warning:
                    return SeverityLevel.Warning;
                case LogLevel.Information:
                    return SeverityLevel.Information;
                case LogLevel.Debug:
                case LogLevel.Trace:
                default:
                    return SeverityLevel.Verbose;
            }
        }

        private void PopulateTelemetry(ITelemetry telemetry, IReadOnlyList<KeyValuePair<string, object>> stateDictionary, EventId eventId)
        {
            if (telemetry is ISupportProperties telemetryWithProperties)
            {
                IDictionary<string, string> dict = telemetryWithProperties.Properties;
                dict["CategoryName"] = this.categoryName;

                if (this.options?.IncludeEventId ?? false)
                {
                    if (eventId.Id != 0)
                    {
                        dict["EventId"] = eventId.Id.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }

                    if (!string.IsNullOrEmpty(eventId.Name))
                    {
                        dict["EventName"] = eventId.Name;
                    }
                }

                if (stateDictionary != null)
                {
                    foreach (KeyValuePair<string, object> item in stateDictionary)
                    {
                        dict[item.Key] = Convert.ToString(item.Value, CultureInfo.InvariantCulture);
                    }
                }
            }

            telemetry.Context.GetInternalContext().SdkVersion = this.sdkVersion;
        }
    }
#pragma warning restore CS0618
}
