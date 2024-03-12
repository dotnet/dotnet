// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsAppender.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.Log4NetAppender
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    
    using log4net.Appender;
    using log4net.Core;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Implementation;

    /// <summary>
    /// Log4Net Appender that routes all logging output to the Application Insights logging framework.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "Releasing the resources on the close method")]
    public sealed class ApplicationInsightsAppender : AppenderSkeleton
    {
        private TelemetryClient telemetryClient;

        /// <summary>
        /// Gets or sets The Application Insights instrumentationKey for your application. 
        /// </summary>
        /// <remarks>
        /// This is normally pushed from when Appender is being initialized.
        /// </remarks>
        public string InstrumentationKey { get; set; }

        internal TelemetryClient TelemetryClient
        {
            get { return this.telemetryClient; }
        }

        /// <summary>
        /// Gets a value indicating whether layout is required. The <see cref="ApplicationInsightsAppender"/> requires a layout.
        /// This Appender converts the LoggingEvent it receives into a text string and requires the layout format string to do so.
        /// </summary>
        protected override bool RequiresLayout
        {
            get { return true; }
        }

        /// <summary>
        /// Initializes the Appender and perform instrumentationKey validation.
        /// </summary>
        public override void ActivateOptions()
        {
            base.ActivateOptions();
#pragma warning disable CS0618 // Type or member is obsolete
            this.telemetryClient = new TelemetryClient();
#pragma warning restore CS0618 // Type or member is obsolete
            if (!string.IsNullOrEmpty(this.InstrumentationKey))
            {
                this.telemetryClient.Context.InstrumentationKey = this.InstrumentationKey;
            }

            this.telemetryClient.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("log4net:");
        }

        /// <summary>
        /// Flushes any buffered log data.
        /// </summary>
        /// <param name="millisecondsTimeout">The maximum time to wait for logging events to be flushed.</param>
        /// <returns>True if all logging events were flushed successfully, else false.</returns>
        public override bool Flush(int millisecondsTimeout)
        {
            this.telemetryClient.Flush();
            return true;
        }

        /// <summary>
        /// Append LoggingEvent Application Insights logging framework.
        /// </summary>
        /// <param name="loggingEvent">Events to be logged.</param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException(nameof(loggingEvent));
            }

            if (loggingEvent.ExceptionObject != null)
            {
                this.SendException(loggingEvent);
            }
            else
            {
                this.SendTrace(loggingEvent);
            }
        }

        private static void AddLoggingEventProperty(string key, string value, IDictionary<string, string> metaData)
        {
            if (value != null && !metaData.ContainsKey(key))
            {
                metaData.Add(key, value);
            }
        }

        private static void BuildCustomProperties(LoggingEvent loggingEvent, ITelemetry trace)
        {
            trace.Timestamp = loggingEvent.TimeStamp;

            IDictionary<string, string> metaData;
            
            if (trace is ExceptionTelemetry)
            {
                metaData = ((ExceptionTelemetry)trace).Properties;
            }
            else
            {
                metaData = ((TraceTelemetry)trace).Properties;
            }

            AddLoggingEventProperty("LoggerName", loggingEvent.LoggerName, metaData);
            AddLoggingEventProperty("ThreadName", loggingEvent.ThreadName, metaData);

            var locationInformation = loggingEvent.LocationInformation;
            if (locationInformation != null)
            {
                AddLoggingEventProperty("ClassName", locationInformation.ClassName, metaData);
                AddLoggingEventProperty("FileName", locationInformation.FileName, metaData);
                AddLoggingEventProperty("MethodName", locationInformation.MethodName, metaData);
                AddLoggingEventProperty("LineNumber", locationInformation.LineNumber, metaData);
            }
            
            AddLoggingEventProperty("Domain", loggingEvent.Domain, metaData);
            AddLoggingEventProperty("Identity", loggingEvent.Identity, metaData);

            var properties = loggingEvent.GetProperties();
            if (properties != null)
            {
                foreach (string key in properties.GetKeys())
                {
                    if (!string.IsNullOrEmpty(key) && !key.StartsWith("log4net", StringComparison.OrdinalIgnoreCase))
                    {
                        object value = properties[key];
                        if (value != null)
                        {
                            AddLoggingEventProperty(key, value.ToString(), metaData);
                        }
                    }
                }
            }
        }

        private static SeverityLevel? GetSeverityLevel(Level logginEventLevel)
        {
            if (logginEventLevel == null)
            {
                return null;
            }

            if (logginEventLevel.Value < Level.Info.Value)
            {
                return SeverityLevel.Verbose;
            }

            if (logginEventLevel.Value < Level.Warn.Value)
            {
                return SeverityLevel.Information;
            }

            if (logginEventLevel.Value < Level.Error.Value)
            {
                return SeverityLevel.Warning;
            }

            if (logginEventLevel.Value < Level.Severe.Value)
            {
                return SeverityLevel.Error;
            }

            return SeverityLevel.Critical;
        }

        private void SendException(LoggingEvent loggingEvent)
        {
            try
            {
                var exceptionTelemetry = new ExceptionTelemetry(loggingEvent.ExceptionObject)
                {
                    SeverityLevel = GetSeverityLevel(loggingEvent.Level),
                    Message = $"{loggingEvent.ExceptionObject.GetType().ToString()}: {loggingEvent.ExceptionObject.Message}",
                };

                if (loggingEvent.RenderedMessage != null)
                {
                    var message = this.RenderLoggingEvent(loggingEvent); 
                    if (!string.IsNullOrEmpty(message))
                    {
                        exceptionTelemetry.Properties.Add("Message", message);
                    }
                }

                BuildCustomProperties(loggingEvent, exceptionTelemetry);
                this.telemetryClient.Track(exceptionTelemetry);
            }
            catch (ArgumentNullException exception)
            {
                throw new LogException(exception.Message, exception);
            }
        }

        private void SendTrace(LoggingEvent loggingEvent)
        {
            try
            {
                string message = loggingEvent.RenderedMessage != null ? this.RenderLoggingEvent(loggingEvent) : "Log4Net Trace";

                var trace = new TraceTelemetry(message)
                {
                    SeverityLevel = GetSeverityLevel(loggingEvent.Level),
                };

                BuildCustomProperties(loggingEvent, trace);
                this.telemetryClient.Track(trace);
            }
            catch (ArgumentNullException exception)
            {
                throw new LogException(exception.Message, exception);
            }
        }
    }
}
