// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;
using Microsoft.IdentityModel.Abstractions;

namespace Microsoft.IdentityModel.Logging
{
    /// <summary>
    /// Helper class for logging.
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// Gets or sets a logger to which logs will be written to.
        /// </summary>
        public static IIdentityLogger Logger { get; set; } = NullIdentityModelLogger.Instance;

        /// <summary>
        /// Indicates whether the log message header (contains library version, date/time, and PII debugging information) has been written.
        /// </summary>
        private static bool _isHeaderWritten = false;

        /// <summary>
        /// The log message that is shown when PII is off.
        /// </summary>
        private static string _piiOffLogMessage = "PII logging is OFF. See https://aka.ms/IdentityModel/PII for details. ";

        /// <summary>
        /// The log message that is shown when PII is on.
        /// </summary>
        private static string _piiOnLogMessage = "PII logging is ON, do not use in production. See https://aka.ms/IdentityModel/PII for details. ";

        // internal for testing purposes only
        internal static bool HeaderWritten
        {
            get { return _isHeaderWritten; }
            set { _isHeaderWritten = value; }
        }

        /// <summary>
        /// Gets whether logging is enabled at the specified <see cref="EventLogLevel"/>."/>
        /// </summary>
        /// <param name="level">The log level</param>
        /// <returns><see langword="true"/> if logging is enabled at the specified level; otherwise, <see langword="false"/>.</returns>
        public static bool IsEnabled(EventLogLevel level) =>
            Logger.IsEnabled(level) ||
            IdentityModelEventSource.Logger.IsEnabled(EventLogLevelToEventLevel(level), EventKeywords.All);

        /// <summary>
        /// Logs an exception using the event source logger and returns new <see cref="ArgumentNullException"/> exception.
        /// </summary>
        /// <param name="argument">argument that is null or empty.</param>
        /// <remarks>EventLevel is set to Error.</remarks>
        public static ArgumentNullException LogArgumentNullException(string argument)
        {
            return LogArgumentException<ArgumentNullException>(EventLevel.Error, argument, "IDX10000: The parameter '{0}' cannot be a 'null' or an empty object. ", argument);
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="message">message to log.</param>
        /// <remarks>EventLevel is set to Error.</remarks>
        public static T LogException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string message) where T : Exception
        {
            return LogException<T>(EventLevel.Error, null, message, null);
        }

        /// <summary>
        /// Logs an argument exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="argumentName">Identifies the argument whose value generated the ArgumentException.</param>
        /// <param name="message">message to log.</param>
        /// <remarks>EventLevel is set to Error.</remarks>
        public static T LogArgumentException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string argumentName, string message) where T : ArgumentException
        {
            return LogArgumentException<T>(EventLevel.Error, argumentName, null, message, null);
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="format">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <remarks>EventLevel is set to Error.</remarks>
        public static T LogException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string format, params object[] args) where T : Exception
        {
            return LogException<T>(EventLevel.Error, null, format, args);
        }

        /// <summary>
        /// Logs an argument exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="argumentName">Identifies the argument whose value generated the ArgumentException.</param>
        /// <param name="format">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <remarks>EventLevel is set to Error.</remarks>
        public static T LogArgumentException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string argumentName, string format, params object[] args) where T : ArgumentException
        {
            return LogArgumentException<T>(EventLevel.Error, argumentName, null, format, args);
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="innerException">the inner <see cref="Exception"/> to be added to the outer exception.</param>
        /// <param name="message">message to log.</param>
        /// <remarks>EventLevel is set to Error.</remarks>
        public static T LogException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(Exception innerException, string message) where T : Exception
        {
            return LogException<T>(EventLevel.Error, innerException, message, null);
        }

        /// <summary>
        /// Logs an argument exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="argumentName">Identifies the argument whose value generated the ArgumentException.</param>
        /// <param name="innerException">the inner <see cref="Exception"/> to be added to the outer exception.</param>
        /// <param name="message">message to log.</param>
        /// <remarks>EventLevel is set to Error.</remarks>
        public static T LogArgumentException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string argumentName, Exception innerException, string message) where T : ArgumentException
        {
            return LogArgumentException<T>(EventLevel.Error, argumentName, innerException, message, null);
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="innerException">the inner <see cref="Exception"/> to be added to the outer exception.</param>
        /// <param name="format">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <remarks>EventLevel is set to Error.</remarks>
        public static T LogException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(Exception innerException, string format, params object[] args) where T : Exception
        {
            return LogException<T>(EventLevel.Error, innerException, format, args);
        }

        /// <summary>
        /// Logs an argument exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="argumentName">Identifies the argument whose value generated the ArgumentException.</param>
        /// <param name="innerException">the inner <see cref="Exception"/> to be added to the outer exception.</param>
        /// <param name="format">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <remarks>EventLevel is set to Error.</remarks>
        public static T LogArgumentException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string argumentName, Exception innerException, string format, params object[] args) where T : ArgumentException
        {
            return LogArgumentException<T>(EventLevel.Error, argumentName, innerException, format, args);
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="message">message to log.</param>
        public static T LogException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(EventLevel eventLevel, string message) where T : Exception
        {
            return LogException<T>(eventLevel, null, message, null);
        }

        /// <summary>
        /// Logs an argument exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="argumentName">Identifies the argument whose value generated the ArgumentException.</param>
        /// <param name="message">message to log.</param>
        public static T LogArgumentException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(EventLevel eventLevel, string argumentName, string message) where T : ArgumentException
        {
            return LogArgumentException<T>(eventLevel, argumentName, null, message, null);
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="format">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static T LogException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(EventLevel eventLevel, string format, params object[] args) where T : Exception
        {
            return LogException<T>(eventLevel, null, format, args);
        }

        /// <summary>
        /// Logs an argument exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="argumentName">Identifies the argument whose value generated the ArgumentException.</param>
        /// <param name="format">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static T LogArgumentException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(EventLevel eventLevel, string argumentName, string format, params object[] args) where T : ArgumentException
        {
            return LogArgumentException<T>(eventLevel, argumentName, null, format, args);
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="innerException">the inner <see cref="Exception"/> to be added to the outer exception.</param>
        /// <param name="message">message to log.</param>
        public static T LogException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(EventLevel eventLevel, Exception innerException, string message) where T : Exception
        {
            return LogException<T>(eventLevel, innerException, message, null);
        }

        /// <summary>
        /// Logs an argument exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="argumentName">Identifies the argument whose value generated the ArgumentException.</param>
        /// <param name="innerException">the inner <see cref="Exception"/> to be added to the outer exception.</param>
        /// <param name="message">message to log.</param>
        public static T LogArgumentException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(EventLevel eventLevel, string argumentName, Exception innerException, string message) where T : ArgumentException
        {
            return LogArgumentException<T>(eventLevel, argumentName, innerException, message, null);
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="innerException">the inner <see cref="Exception"/> to be added to the outer exception.</param>
        /// <param name="format">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static T LogException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(EventLevel eventLevel, Exception innerException, string format, params object[] args) where T : Exception
        {
            return LogExceptionImpl<T>(eventLevel, null, innerException, format, args);
        }

        /// <summary>
        /// Logs an argument exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="argumentName">Identifies the argument whose value generated the ArgumentException.</param>
        /// <param name="innerException">the inner <see cref="Exception"/> to be added to the outer exception.</param>
        /// <param name="format">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static T LogArgumentException<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(EventLevel eventLevel, string argumentName, Exception innerException, string format, params object[] args) where T : ArgumentException
        {
            return LogExceptionImpl<T>(eventLevel, argumentName, innerException, format, args);
        }

        /// <summary>
        /// Logs an exception using the event source logger.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        public static Exception LogExceptionMessage(Exception exception)
        {
            return LogExceptionMessage(EventLevel.Error, exception);
        }

        /// <summary>
        /// Logs an exception using the event source logger.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="exception">The exception to log.</param>
        public static Exception LogExceptionMessage(EventLevel eventLevel, Exception exception)
        {
            if (exception == null)
                return null;

            if (IdentityModelEventSource.Logger.IsEnabled(eventLevel, EventKeywords.All))
                IdentityModelEventSource.Logger.Write(eventLevel, exception.InnerException, exception.Message);

            EventLogLevel eventLogLevel = EventLevelToEventLogLevel(eventLevel);
            if (Logger.IsEnabled(eventLogLevel))
                Logger.Log(WriteEntry(eventLogLevel, exception.InnerException, exception.Message, null));

            return exception;
        }

        /// <summary>
        /// Logs an information event.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogInformation(string message, params object[] args)
        {
            if (IdentityModelEventSource.Logger.IsEnabled(EventLevel.Informational, EventKeywords.All))
                IdentityModelEventSource.Logger.WriteInformation(message, args);

            if (Logger.IsEnabled(EventLogLevel.Informational))
                Logger.Log(WriteEntry(EventLogLevel.Informational, null, message, args));
        }

        /// <summary>
        /// Logs a verbose event.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogVerbose(string message, params object[] args)
        {
            if (IdentityModelEventSource.Logger.IsEnabled(EventLevel.Verbose, EventKeywords.All))
                    IdentityModelEventSource.Logger.WriteVerbose(message, args);

            if (Logger.IsEnabled(EventLogLevel.Verbose))
                Logger.Log(WriteEntry(EventLogLevel.Verbose, null, message, args));
        }

        /// <summary>
        /// Logs a warning event.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogWarning(string message, params object[] args)
        {
            if (IdentityModelEventSource.Logger.IsEnabled(EventLevel.Warning, EventKeywords.All))
                    IdentityModelEventSource.Logger.WriteWarning(message, args);

            if (Logger.IsEnabled(EventLogLevel.Warning))
                Logger.Log(WriteEntry(EventLogLevel.Warning, null, message, args));
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="argumentName">Identifies the argument whose value generated the ArgumentException.</param>
        /// <param name="innerException">the inner <see cref="Exception"/> to be added to the outer exception.</param>
        /// <param name="format">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        private static T LogExceptionImpl<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(EventLevel eventLevel, string argumentName, Exception innerException, string format, params object[] args) where T : Exception 
        {
            string message;
            if (args != null)
                message = string.Format(CultureInfo.InvariantCulture, format, args);
            else
                message = format;

            if (IdentityModelEventSource.Logger.IsEnabled(eventLevel, EventKeywords.All))
                IdentityModelEventSource.Logger.Write(eventLevel, innerException, message);

            EventLogLevel eventLogLevel = EventLevelToEventLogLevel(eventLevel);
            if (Logger.IsEnabled(eventLogLevel))
                Logger.Log(WriteEntry(eventLogLevel, innerException, message, null));

            if (innerException != null) 
                if (string.IsNullOrEmpty(argumentName))
                    return (T)Activator.CreateInstance(typeof(T), message, innerException);
                else
                    return (T)Activator.CreateInstance(typeof(T), argumentName, message, innerException);
            else
                if (string.IsNullOrEmpty(argumentName))
                    return (T)Activator.CreateInstance(typeof(T), message);
                else
                    return (T)Activator.CreateInstance(typeof(T), argumentName, message);
        }

        private static EventLogLevel EventLevelToEventLogLevel(EventLevel eventLevel) =>
            (uint)(int)eventLevel <= 5 ? (EventLogLevel)eventLevel : EventLogLevel.Error;

        private static EventLevel EventLogLevelToEventLevel(EventLogLevel eventLevel) =>
            (uint)(int)eventLevel <= 5 ? (EventLevel)eventLevel : EventLevel.Error;

        /// <summary>
        /// Formats the string using InvariantCulture
        /// </summary>
        /// <param name="format">Format string.</param>
        /// <param name="args">Format arguments.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatInvariant(string format, params object[] args)
        {
            if (format == null)
                return string.Empty;

            if (args == null)
                return format;

            if (!IdentityModelEventSource.ShowPII)
                return string.Format(CultureInfo.InvariantCulture, format, args.Select(RemovePII).ToArray());
            else
                return string.Format(CultureInfo.InvariantCulture, format, args.Select(SanitizeSecurityArtifact).ToArray());
        }

        private static object SanitizeSecurityArtifact(object arg)
        {
            if (arg == null)
                return "null";

            if (IdentityModelEventSource.LogCompleteSecurityArtifact && arg is ISafeLogSecurityArtifact)
                return (arg as ISafeLogSecurityArtifact).UnsafeToString();

            return arg;
        }

        private static string RemovePII(object arg)
        {
            if (arg is Exception ex && IsCustomException(ex))
                return ex.ToString();

            if (arg is NonPII)
                return arg.ToString();

            return string.Format(CultureInfo.InvariantCulture, IdentityModelEventSource.HiddenPIIString, arg?.GetType().ToString() ?? "Null");
        }

        internal static bool IsCustomException(Exception ex)
        {
            return ex.GetType().FullName.StartsWith("Microsoft.IdentityModel.", StringComparison.Ordinal);
        }

        /// <summary>
        /// Marks a log message argument (<paramref name="arg"/>) as NonPII.
        /// </summary>
        /// <param name="arg">A log message argument to be marked as NonPII.</param>
        /// <returns>An argument marked as NonPII.</returns>
        /// <remarks>
        /// Marking an argument as NonPII in <see cref="LogHelper.FormatInvariant"/> calls will result in logging
        /// that argument in cleartext, regardless of the <see cref="IdentityModelEventSource.ShowPII"/> flag value.
        /// </remarks>
        public static object MarkAsNonPII(object arg)
        {
            return new NonPII(arg);
        }

        /// <summary>
        /// Marks a log message argument (<paramref name="arg"/>) as SecurityArtifact.
        /// </summary>
        /// <param name="arg">A log message argument to be marked as SecurityArtifact.</param>
        /// <param name="callback">A callback function to log the security artifact safely.</param>
        /// <returns>An argument marked as SecurityArtifact.</returns>
        public static object MarkAsSecurityArtifact(object arg, Func<object, string> callback)
        {
            return new SecurityArtifact(arg, callback);
        }

        /// <summary>
        /// Creates a <see cref="LogEntry"/> by using the provided event level, exception argument, string argument and arguments list.
        /// </summary>
        /// <param name="eventLogLevel"><see cref="EventLogLevel"/></param>
        /// <param name="innerException"><see cref="Exception"/></param>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        private static LogEntry WriteEntry(EventLogLevel eventLogLevel, Exception innerException, string message, params object[] args)
        {
            if (string.IsNullOrEmpty(message))
                return null;

            if (innerException != null)
            {
                // if PII is turned off and 'innerException' is a System exception only display the exception type
                if (!IdentityModelEventSource.ShowPII && !LogHelper.IsCustomException(innerException))
                    message = string.Format(CultureInfo.InvariantCulture, "Message: {0}, InnerException: {1}. ", message, innerException.GetType());
                else // otherwise it's safe to display the entire exception message
                    message = string.Format(CultureInfo.InvariantCulture, "Message: {0}, InnerException: {1}. ", message, innerException.Message);
            }

            message = args == null ? message : FormatInvariant(message, args);

            LogEntry entry = new LogEntry();
            entry.EventLogLevel = eventLogLevel;

            // Prefix header (library version, DateTime, whether PII is ON/OFF) to the first message logged by Wilson.
            if (!_isHeaderWritten)
            {
                string headerMessage = string.Format(CultureInfo.InvariantCulture, "Microsoft.IdentityModel Version: {0}. Date {1}. {2}",
                    typeof(IdentityModelEventSource).Assembly.GetName().Version.ToString(),
                    DateTime.UtcNow,
                    IdentityModelEventSource.ShowPII ? _piiOnLogMessage : _piiOffLogMessage);

                entry.Message = headerMessage + Environment.NewLine + message;

                _isHeaderWritten = true;
            }
            else
                entry.Message = message;

            return entry;
        }
    }
}
