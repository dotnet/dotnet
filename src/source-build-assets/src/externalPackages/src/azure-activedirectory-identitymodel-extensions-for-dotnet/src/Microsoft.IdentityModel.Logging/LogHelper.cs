// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Abstractions;
#if NET8_0_OR_GREATER
using System.Buffers;
#endif

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
        private static bool _isHeaderWritten;

#if NET8_0_OR_GREATER
        /// <summary>
        /// SearchValues containing all characters that need to be sanitized in log output.
        /// This includes all control characters (Unicode category Cc) and format characters (Unicode category Cf).
        /// </summary>
        private static readonly SearchValues<char> s_charsToSanitize = SearchValues.Create(new char[] {
            '\u0000', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006', '\u0007', '\u0008', '\u0009', '\u000A', '\u000B', '\u000C', '\u000D', '\u000E', '\u000F',
            '\u0010', '\u0011', '\u0012', '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019', '\u001A', '\u001B', '\u001C', '\u001D', '\u001E', '\u001F',
            '\u007F', '\u0080', '\u0081', '\u0082', '\u0083', '\u0084', '\u0085', '\u0086', '\u0087', '\u0088', '\u0089', '\u008A', '\u008B', '\u008C', '\u008D', '\u008E',
            '\u008F', '\u0090', '\u0091', '\u0092', '\u0093', '\u0094', '\u0095', '\u0096', '\u0097', '\u0098', '\u0099', '\u009A', '\u009B', '\u009C', '\u009D', '\u009E',
            '\u009F', '\u00AD', '\u0600', '\u0601', '\u0602', '\u0603', '\u0604', '\u0605', '\u061C', '\u06DD', '\u070F', '\u0890', '\u0891', '\u08E2', '\u180E', '\u200B',
            '\u200C', '\u200D', '\u200E', '\u200F', '\u202A', '\u202B', '\u202C', '\u202D', '\u202E', '\u2060', '\u2061', '\u2062', '\u2063', '\u2064', '\u2066', '\u2067',
            '\u2068', '\u2069', '\u206A', '\u206B', '\u206C', '\u206D', '\u206E', '\u206F', '\uFEFF', '\uFFF9', '\uFFFA', '\uFFFB'
        });
#endif

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
            {
                if (string.IsNullOrEmpty(argumentName))
                    return (T)Activator.CreateInstance(typeof(T), message, innerException);
                else
                    return (T)Activator.CreateInstance(typeof(T), argumentName, message, innerException);
            }
            else
            {
                if (string.IsNullOrEmpty(argumentName))
                    return (T)Activator.CreateInstance(typeof(T), message);
                else
                    return (T)Activator.CreateInstance(typeof(T), argumentName, message);
            }
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
            else if (arg is ISafeLogSecurityArtifact)
            {
                // We may later add a further flag which would log a best effort scrubbing of an artifact. E.g. JsonWebToken tries to remove the signature
                // in the current implementation. Another flag may be added in the future to allow this middle path but for now, LogCompleteSecurityArtifact
                // must be logged to emit any token part (other than specific claim values).
                return string.Format(CultureInfo.InvariantCulture, IdentityModelEventSource.HiddenSecurityArtifactString, arg?.GetType().ToString() ?? "Null");
            }

            // If it's not a ISafeLogSecurityArtifact then just return the object which will be converted to string.
            // It's possible a raw string will contain a security artifact and be exposed here but the alternative is to scrub all objects
            // which defeats the purpose of the ShowPII flag.
            return Sanitize(arg.ToString()); //Sanitizes PII strings when ShowPII is true.
        }

        private static string RemovePII(object arg)
        {
            if (arg is Exception ex && IsCustomException(ex))
                return ex.ToString();

            if (arg is NonPII)
                return Sanitize(arg.ToString()); // Sanitizes non-PII

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
        /// <remarks>
        /// Since even the payload may sometimes contain security artifacts, naïve disarm algorithms such as removing signatures
        /// will not work. For now the <paramref name="callback"/> will only be leveraged if
        /// <see cref="IdentityModelEventSource.LogCompleteSecurityArtifact"/> is set and no unsafe callback is provided. Future changes
        /// may introduce a support for best effort disarm logging.
        /// </remarks>
        public static object MarkAsSecurityArtifact(object arg, Func<object, string> callback)
        {
            return new SecurityArtifact(arg, callback);
        }

        /// <summary>
        /// Marks a log message argument (<paramref name="arg"/>) as SecurityArtifact.
        /// </summary>
        /// <param name="arg">A log message argument to be marked as SecurityArtifact.</param>
        /// <param name="callback">A callback function to log the security artifact safely.</param>
        /// <param name="callbackUnsafe">A callback function to log the security artifact without scrubbing.</param>
        /// <returns>An argument marked as SecurityArtifact.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="callback"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="callbackUnsafe"/> is null.</exception>
        /// <remarks>
        /// Since even the payload may sometimes contain security artifacts, naïve disarm algorithms such as removing signatures
        /// will not work. For now the <paramref name="callback"/> is currently unused. Future changes
        /// may introduce a support for best effort disarm logging which will leverage <paramref name="callback"/>.
        /// </remarks>
        public static object MarkAsSecurityArtifact(object arg, Func<object, string> callback, Func<object, string> callbackUnsafe)
        {
            return new SecurityArtifact(arg, callback, callbackUnsafe);
        }

        /// <summary>
        /// Marks a log message argument (<paramref name="arg"/>) as SecurityArtifact.
        /// </summary>
        /// <param name="arg">A log message argument to be marked as SecurityArtifact.</param>
        /// <param name="callbackUnsafe">A callback function to log the security artifact without scrubbing.</param>
        /// <returns>An argument marked as SecurityArtifact.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="callbackUnsafe"/> is null.</exception>
        public static object MarkAsUnsafeSecurityArtifact(object arg, Func<object, string> callbackUnsafe)
        {
            return new SecurityArtifact(arg, SecurityArtifact.UnknownSafeTokenCallback, callbackUnsafe);
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

        /// <summary>
        /// Sanitizes a string by encoding potentially harmful characters.
        /// </summary>
        /// <param name="input">The input string to sanitize</param>
        /// <returns>A sanitized string safe for logging</returns>
        private static string Sanitize(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

#if NET8_0_OR_GREATER
            // Use SearchValues for efficient character searching on .NET 8+
            int index = input.AsSpan().IndexOfAny(s_charsToSanitize);
            if (index < 0)
                return input; // No characters to sanitize

            var sanitized = new StringBuilder(input.Length);
            int lastIndex = 0;

            while (index >= 0)
            {
                // Append the part before the character to sanitize
                sanitized.Append(input.AsSpan(lastIndex, index - lastIndex));

                char c = input[index];
                if (c == '\r')
                    sanitized.Append("\\r");
                else if (c == '\n')
                    sanitized.Append("\\n");
                else if (c == '\t')
                    sanitized.Append("\\t");
                else
                    sanitized.Append($"\\u{(int)c:X4}");

                lastIndex = index + 1;

                // Find next character to sanitize
                if (lastIndex < input.Length)
                    index = input.AsSpan(lastIndex).IndexOfAny(s_charsToSanitize);
                else
                    index = -1;

                if (index >= 0)
                    index += lastIndex; // Adjust index to be relative to the original string
            }

            // Append any remaining characters
            if (lastIndex < input.Length)
                sanitized.Append(input.AsSpan(lastIndex));

            return sanitized.ToString();
#else
            // Fallback for older .NET versions
            var sanitized = new StringBuilder(input.Length);

            foreach (char c in input)
            {
                if (c == '\r')
                    sanitized.Append("\\r");
                else if (c == '\n')
                    sanitized.Append("\\n");
                else if (c == '\t')
                    sanitized.Append("\\t");
                else if (char.IsControl(c) || CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.Format)
                    sanitized.Append($"\\u{(int)c:X4}");
                else
                    sanitized.Append(c);
            }

            return sanitized.ToString();
#endif
        }
    }
}
