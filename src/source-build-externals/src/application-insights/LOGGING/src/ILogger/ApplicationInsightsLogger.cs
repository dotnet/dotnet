// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsLogger.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// All rights reserved.  2013
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Extensions.Logging.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Application insights logger implementation for <see cref="ILogger"/>.
    /// </summary>
    /// <seealso cref="ILogger" />
    public class ApplicationInsightsLogger : ILogger
    {
        private readonly string categoryName;
        private readonly TelemetryClient telemetryClient;
        private readonly ApplicationInsightsLoggerOptions applicationInsightsLoggerOptions;

        /// <summary>
        /// Creates a new instance of <see cref="ApplicationInsightsLogger"/>.
        /// </summary>
        public ApplicationInsightsLogger(
            string categoryName,
            TelemetryClient telemetryClient,
            ApplicationInsightsLoggerOptions applicationInsightsLoggerOptions)
        {
            this.categoryName = categoryName;
            this.telemetryClient = telemetryClient;
            this.applicationInsightsLoggerOptions = applicationInsightsLoggerOptions ?? throw new ArgumentNullException(nameof(applicationInsightsLoggerOptions));
        }

        /// <summary>
        /// Gets or sets the external scope provider.
        /// </summary>
        internal IExternalScopeProvider ExternalScopeProvider { get; set; }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">Current state.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>
        /// An IDisposable that ends the logical operation scope on dispose.
        /// </returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return this.ExternalScopeProvider != null ? this.ExternalScopeProvider.Push(state) : NullScope.Instance;
        }

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns>
        ///   <c>true</c> if enabled.
        /// </returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return this.telemetryClient.IsEnabled();
        }

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TState">State being passed along.</typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            try
            {
                if (this.IsEnabled(logLevel))
                {
                    if (exception == null || !this.applicationInsightsLoggerOptions.TrackExceptionsAsExceptionTelemetry)
                    {
                        TraceTelemetry traceTelemetry = new TraceTelemetry(
                            formatter(state, exception),
                            ApplicationInsightsLogger.GetSeverityLevel(logLevel));
                        this.PopulateTelemetry(traceTelemetry, state, eventId);
                        if (exception != null)
                        {
                            traceTelemetry.Properties.Add("ExceptionMessage", exception.Message);
                            traceTelemetry.Properties.Add("ExceptionStackTrace", exception.ToInvariantString());
                        }

                        this.telemetryClient.TrackTrace(traceTelemetry);
                    }
                    else
                    {
                        ExceptionTelemetry exceptionTelemetry = new ExceptionTelemetry(exception)
                        {
                            Message = exception.Message,
                            SeverityLevel = ApplicationInsightsLogger.GetSeverityLevel(logLevel),
                        };

                        exceptionTelemetry.Properties.Add("FormattedMessage", formatter(state, exception));
                        this.PopulateTelemetry(exceptionTelemetry, state, eventId);
                        this.telemetryClient.TrackException(exceptionTelemetry);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationInsightsLoggerEventSource.Log.FailedToLog(ex.ToInvariantString());
            }
        }

        /// <summary>
        /// Converts the <see cref="LogLevel"/> into corresponding Application insights <see cref="SeverityLevel"/>.
        /// </summary>
        /// <param name="logLevel">Logging log level.</param>
        /// <returns>Application insights corresponding SeverityLevel for the LogLevel.</returns>
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

        /// <summary>
        /// Populates the state, scope and event information for the logging event.
        /// </summary>
        /// <typeparam name="TState">State information for the current event.</typeparam>
        /// <param name="telemetryItem">Telemetry item.</param>
        /// <param name="state">Event state information.</param>
        /// <param name="eventId">Event Id information.</param>
        private void PopulateTelemetry<TState>(ISupportProperties telemetryItem, TState state, EventId eventId)
        {
            IDictionary<string, string> dict = telemetryItem.Properties;
            dict["CategoryName"] = this.categoryName;

            if (eventId.Id != 0)
            {
                dict["EventId"] = eventId.Id.ToString(CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(eventId.Name))
            {
                dict["EventName"] = eventId.Name;
            }

            if (state is IReadOnlyCollection<KeyValuePair<string, object>> stateDictionary)
            {
                foreach (KeyValuePair<string, object> item in stateDictionary)
                {
                    if (item.Key == "{OriginalFormat}")
                    {
                        dict["OriginalFormat"] = Convert.ToString(item.Value, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        dict[item.Key] = Convert.ToString(item.Value, CultureInfo.InvariantCulture);
                    }
                }
            }

            if (this.applicationInsightsLoggerOptions.IncludeScopes)
            {
                if (this.ExternalScopeProvider != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    this.ExternalScopeProvider.ForEachScope(
                        (activeScope, builder) =>
                        {
                            // Ideally we expect that the scope to implement IReadOnlyList<KeyValuePair<string, object>>.
                            // But this is not guaranteed as user can call BeginScope and pass anything. Hence
                            // we try to resolve the scope as Dictionary and if we fail, we just serialize the object and add it.

                            if (activeScope is IReadOnlyCollection<KeyValuePair<string, object>> activeScopeDictionary)
                            {
                                foreach (KeyValuePair<string, object> item in activeScopeDictionary)
                                {
                                    if (item.Key == "{OriginalFormat}")
                                    {
                                        dict["OriginalFormat"] = Convert.ToString(item.Value, CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        dict[item.Key] = Convert.ToString(item.Value, CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                            else
                            {
                                builder.Append(" => ").Append(activeScope);
                            }
                        },
                        stringBuilder);

                    if (stringBuilder.Length > 0)
                    {
                        dict["Scope"] = stringBuilder.ToString();
                    }
                }
            }
        }
    }
}
