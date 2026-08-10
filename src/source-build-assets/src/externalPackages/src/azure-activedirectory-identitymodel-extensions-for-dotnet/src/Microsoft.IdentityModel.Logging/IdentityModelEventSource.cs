// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Reflection;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Logging
{
    /// <summary>
    /// Event source based logger to log different events.
    /// </summary>
    [EventSource(Name = "Microsoft.IdentityModel.EventSource")]
    public class IdentityModelEventSource : EventSource
    {
        static IdentityModelEventSource()
        {
            Logger = new IdentityModelEventSource();
        }

        private IdentityModelEventSource()
        {
            LogLevel = EventLevel.Warning;
        }

        /// <summary>
        /// Static logger that is exposed externally. An external application or framework can hook up a listener to this event source to log data in a custom way.
        /// </summary>
        public static IdentityModelEventSource Logger { get; }

        /// <summary>
        /// Flag which indicates whether or not PII is shown in logs. False by default.
        /// </summary>
        public static bool ShowPII { get; set; }

        /// <summary>
        /// Flag which indicates whether or not complete <see cref="SecurityArtifact"/> is shown in logs when <see cref="ShowPII"/> is set to true. False by default.
        /// </summary>
        public static bool LogCompleteSecurityArtifact { get; set; }

        /// <summary>
        /// String that is used in place of any arguments to log messages if the 'ShowPII' flag is set to false.
        /// </summary>
        public static string HiddenPIIString { get; } = "[PII of type '{0}' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]";

        /// <summary>
        /// String that is used in place of any arguments to log messages if the 'LogCompleteSecurityArtifact' flag is set to false.
        /// </summary>
        public static string HiddenSecurityArtifactString { get; } = "[Security Artifact of type '{0}' is hidden. For more details, see https://aka.ms/IdentityModel/SecurityArtifactLogging.]";

        /// <summary>
        /// Indicates whether or the log message header (contains library version, date/time, and PII debugging information) has been written.
        /// </summary>
        public static bool HeaderWritten { get; set; }

        /// <summary>
        /// The log message that indicates the current library version.
        /// </summary>
        private static string _versionLogMessage = "Library version: {0}.";

        /// <summary>
        /// The log message that indicates the date.
        /// </summary>
        private static string _dateLogMessage = "Date: {0}.";

        /// <summary>
        /// The log message that is shown when PII is off.
        /// </summary>
        private static string _piiOffLogMessage = "PII (personally identifiable information) logging is currently turned off. Set IdentityModelEventSource.ShowPII to 'true' to view the full details of exceptions.";

        /// <summary>
        /// The log message that is shown when PII is off.
        /// </summary>
        private static string _piiOnLogMessage = "PII (personally identifiable information) logging is currently turned on. Set IdentityModelEventSource.ShowPII to 'false' to hide PII from log messages.";


        /// <summary>
        /// Writes an event log by using the provided string argument and current UTC time.
        /// No level filtering is done on the event.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <remarks>No level filtering.</remarks>
        [Event(6, Level = EventLevel.LogAlways)]
        public void WriteAlways(string message)
        {
            if (IsEnabled())
            {
                message = PrepareMessage(EventLevel.LogAlways, message);
                WriteEvent(6, message);
            }
        }

        /// <summary>
        /// Writes an event log by using the provided string argument, current UTC time and the provided arguments list.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        [NonEvent]
        public void WriteAlways(string message, params object[] args)
        {
            if (IsEnabled())
            {
                if (args != null)
                    WriteAlways(FormatInvariant(message, args));
                else
                    WriteAlways(message);
            }
        }

        /// <summary>
        /// Writes a verbose event log by using the provided string argument and current UTC time.
        /// </summary>
        /// <param name="message">The log message.</param>
        [Event(1, Level = EventLevel.Verbose)]
        public void WriteVerbose(string message)
        {
            if (IsEnabled() && LogLevel >= EventLevel.Verbose)
            {
                message = PrepareMessage(EventLevel.Verbose, message);
                WriteEvent(1, message);
            }
        }

        /// <summary>
        /// Writes a verbose event log by using the provided string argument, current UTC time and the provided arguments list.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        [NonEvent]
        public void WriteVerbose(string message, params object[] args)
        {
            if (IsEnabled() && LogLevel >= EventLevel.Verbose)
            {
                if (args != null)
                    WriteVerbose(FormatInvariant(message, args));
                else
                    WriteVerbose(message);
            }
        }

        /// <summary>
        /// Writes an information event log by using the provided string argument and current UTC time.
        /// </summary>
        /// <param name="message">The log message.</param>
        [Event(2, Level = EventLevel.Informational)]
        public void WriteInformation(string message)
        {
            if (IsEnabled() && LogLevel >= EventLevel.Informational)
            {
                message = PrepareMessage(EventLevel.Informational, message);
                WriteEvent(2, message);
            }
        }

        /// <summary>
        /// Writes an information event log by using the provided string argument, current UTC time and the provided arguments list.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        [NonEvent]
        public void WriteInformation(string message, params object[] args)
        {
            if (IsEnabled() && LogLevel >= EventLevel.Informational)
            {
                if (args != null)
                    WriteInformation(FormatInvariant(message, args));
                else
                    WriteInformation(message);
            }
        }

        /// <summary>
        /// Writes a warning event log by using the provided string argument and current UTC time.
        /// </summary>
        /// <param name="message">The log message.</param>
        [Event(3, Level = EventLevel.Warning)]
        public void WriteWarning(string message)
        {
            if (IsEnabled() && LogLevel >= EventLevel.Warning)
            {
                message = PrepareMessage(EventLevel.Warning, message);
                WriteEvent(3, message);
            }
        }

        /// <summary>
        /// Writes a warning event log by using the provided string argument, current UTC time and the provided arguments list.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        [NonEvent]
        public void WriteWarning(string message, params object[] args)
        {
            if (args != null)
                WriteWarning(FormatInvariant(message, args));
            else
                WriteWarning(message);
        }

        /// <summary>
        /// Writes an error event log by using the provided string argument and current UTC time.
        /// </summary>
        /// <param name="message">The log message.</param>
        [Event(4, Level = EventLevel.Error)]
        public void WriteError(string message)
        {
            if (IsEnabled() && LogLevel >= EventLevel.Error)
            {
                message = PrepareMessage(EventLevel.Error, message);
                WriteEvent(4, message);
            }
        }

        /// <summary>
        /// Writes an error event log by using the provided string argument, current UTC time and the provided arguments list.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        [NonEvent]
        public void WriteError(string message, params object[] args)
        {
            if (IsEnabled() && LogLevel >= EventLevel.Error)
            {
                if (args != null)
                    WriteError(FormatInvariant(message, args));
                else
                    WriteError(message);
            }
        }

        /// <summary>
        /// Writes a critical event log by using the provided string argument and current UTC time.
        /// </summary>
        /// <param name="message">The log message.</param>
        [Event(5, Level = EventLevel.Critical)]
        public void WriteCritical(string message)
        {
            if (IsEnabled() && LogLevel >= EventLevel.Critical)
            {
                message = PrepareMessage(EventLevel.Critical, message);
                WriteEvent(5, message);
            }
        }

        /// <summary>
        /// Writes a critical event log by using the provided string argument, current UTC time and the provided arguments list.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        [NonEvent]
        public void WriteCritical(string message, params object[] args)
        {
            if (IsEnabled() && LogLevel >= EventLevel.Critical)
            {
                if (args != null)
                    WriteCritical(FormatInvariant(message, args));
                else
                    WriteCritical(message);
            }
        }

        /// <summary>
        /// Writes an exception log by using the provided event identifer, exception argument, string argument and current UTC time.
        /// </summary>
        /// <param name="level"><see cref="EventLevel"/></param>
        /// <param name="innerException"><see cref="Exception"/></param>
        /// <param name="message">The log message.</param>
        [NonEvent]
        public void Write(EventLevel level, Exception innerException, string message)
        {
            Write(level, innerException, message, null);
        }

        /// <summary>
        /// Writes an exception log by using the provided event identifer, exception argument, string argument, arguments list and current UTC time.
        /// </summary>
        /// <param name="level"><see cref="EventLevel"/></param>
        /// <param name="innerException"><see cref="Exception"/></param>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        [NonEvent]
        public void Write(EventLevel level, Exception innerException, string message, params object[] args)
        {
            if (innerException != null)
            {
                // if PII is turned off and 'innerException' is a System exception only display the exception type
                if (!ShowPII && !LogHelper.IsCustomException(innerException))
                    message = string.Format(CultureInfo.InvariantCulture, "Message: {0}, InnerException: {1}", message, innerException.GetType());
                else // otherwise it's safe to display the entire exception message
                    message = string.Format(CultureInfo.InvariantCulture, "Message: {0}, InnerException: {1}", message, innerException.Message);
            }

            // Logs basic information: library version, date, and whether PII (personally identifiable information) logging is on or off.
            if (!HeaderWritten)
            {
                // Obtain the current library version dynamically.
                WriteAlways(string.Format(CultureInfo.InvariantCulture, _versionLogMessage, typeof(IdentityModelEventSource).GetTypeInfo().Assembly.GetName().Version.ToString()));
                WriteAlways(string.Format(CultureInfo.InvariantCulture, _dateLogMessage, DateTime.UtcNow));
                if (ShowPII)
                    WriteAlways(_piiOnLogMessage);
                else
                    WriteAlways(_piiOffLogMessage);

                HeaderWritten = true; // We only want to log this information once before any log messages are written.
            }

            switch (level)
            {
                case EventLevel.LogAlways:
                    WriteAlways(message, args);
                    break;
                case EventLevel.Critical:
                    WriteCritical(message, args);
                    break;
                case EventLevel.Error:
                    WriteError(message, args);
                    break;
                case EventLevel.Warning:
                    WriteWarning(message, args);
                    break;
                case EventLevel.Informational:
                    WriteInformation(message, args);
                    break;
                case EventLevel.Verbose:
                    WriteVerbose(message, args);
                    break;
                default:
                    WriteError(FormatInvariant(LogMessages.MIML10002, level));
                    WriteError(message, args);
                    break;
            }
        }

        /// <summary>
        /// Minimum log level to log events. Default is Warning.
        /// </summary>
        public EventLevel LogLevel
        {
            get; set;
        }

        private static string PrepareMessage(EventLevel level, string message, params object[] args)
        {
            if (message == null)
                return string.Empty;

            try
            {
                if (args != null && args.Length > 0)
                    return string.Format(CultureInfo.InvariantCulture, "[{0}]{1} {2}", level.ToString(), DateTime.UtcNow.ToString(CultureInfo.InvariantCulture), FormatInvariant(message, args));

                return string.Format(CultureInfo.InvariantCulture, "[{0}]{1} {2}", level.ToString(), DateTime.UtcNow.ToString(CultureInfo.InvariantCulture), message);
            }
            catch
            {

            }

            try
            {
                return LogHelper.FormatInvariant("[{0}]{1} {2}", level.ToString(), DateTime.UtcNow.ToString(CultureInfo.InvariantCulture), message);
            }
            catch (Exception)
            {
                return level + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) + message;
            }
        }
    }
}
