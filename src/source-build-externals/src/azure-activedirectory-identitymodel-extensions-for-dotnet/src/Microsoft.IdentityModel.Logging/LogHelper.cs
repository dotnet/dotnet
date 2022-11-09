//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
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
        public static T LogException<T>(string message) where T : Exception
        {
            return LogException<T>(EventLevel.Error, null, message, null);
        }

        /// <summary>
        /// Logs an argument exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="argumentName">Identifies the argument whose value generated the ArgumentException.</param>
        /// <param name="message">message to log.</param>
        /// <remarks>EventLevel is set to Error.</remarks>
        public static T LogArgumentException<T>(string argumentName, string message) where T : ArgumentException
        {
            return LogArgumentException<T>(EventLevel.Error, argumentName, null, message, null);
        }


        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="format">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <remarks>EventLevel is set to Error.</remarks>
        public static T LogException<T>(string format, params object[] args) where T : Exception
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
        public static T LogArgumentException<T>(string argumentName, string format, params object[] args) where T : ArgumentException
        {
            return LogArgumentException<T>(EventLevel.Error, argumentName, null, format, args);
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="innerException">the inner <see cref="Exception"/> to be added to the outer exception.</param>
        /// <param name="message">message to log.</param>
        /// <remarks>EventLevel is set to Error.</remarks>
        public static T LogException<T>(Exception innerException, string message) where T : Exception
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
        public static T LogArgumentException<T>(string argumentName, Exception innerException, string message) where T : ArgumentException
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
        public static T LogException<T>(Exception innerException, string format, params object[] args) where T : Exception
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
        public static T LogArgumentException<T>(string argumentName, Exception innerException, string format, params object[] args) where T : ArgumentException
        {
            return LogArgumentException<T>(EventLevel.Error, argumentName, innerException, format, args);
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="message">message to log.</param>
        public static T LogException<T>(EventLevel eventLevel, string message) where T : Exception
        {
            return LogException<T>(eventLevel, null, message, null);
        }

        /// <summary>
        /// Logs an argument exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="argumentName">Identifies the argument whose value generated the ArgumentException.</param>
        /// <param name="message">message to log.</param>
        public static T LogArgumentException<T>(EventLevel eventLevel, string argumentName, string message) where T : ArgumentException
        {
            return LogArgumentException<T>(eventLevel, argumentName, null, message, null);
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="format">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static T LogException<T>(EventLevel eventLevel, string format, params object[] args) where T : Exception
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
        public static T LogArgumentException<T>(EventLevel eventLevel, string argumentName, string format, params object[] args) where T : ArgumentException
        {
            return LogArgumentException<T>(eventLevel, argumentName, null, format, args);
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="innerException">the inner <see cref="Exception"/> to be added to the outer exception.</param>
        /// <param name="message">message to log.</param>
        public static T LogException<T>(EventLevel eventLevel, Exception innerException, string message) where T : Exception
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
        public static T LogArgumentException<T>(EventLevel eventLevel, string argumentName, Exception innerException, string message) where T : ArgumentException
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
        public static T LogException<T>(EventLevel eventLevel, Exception innerException, string format, params object[] args) where T : Exception
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
        public static T LogArgumentException<T>(EventLevel eventLevel, string argumentName, Exception innerException, string format, params object[] args) where T : ArgumentException
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

            if (IdentityModelEventSource.Logger.IsEnabled() && IdentityModelEventSource.Logger.LogLevel >= eventLevel)
                IdentityModelEventSource.Logger.Write(eventLevel, exception.InnerException, exception.Message);

            EventLogLevel eventLogLevel = Enum.IsDefined(typeof(EventLogLevel), (int)eventLevel) ? (EventLogLevel)eventLevel : EventLogLevel.Error;
            if (Logger.IsEnabled(eventLogLevel))
                Logger.Log(WriteEntry((EventLogLevel)eventLevel, exception.InnerException, exception.Message, null));

            return exception;
        }

        /// <summary>
        /// Logs an information event.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogInformation(string message, params object[] args)
        {
            if (IdentityModelEventSource.Logger.IsEnabled() && IdentityModelEventSource.Logger.LogLevel >= EventLevel.Informational)
                IdentityModelEventSource.Logger.WriteInformation(message, args);

            if (Enum.IsDefined(typeof(EventLogLevel), (int)EventLevel.Informational) && Logger.IsEnabled((EventLogLevel)EventLevel.Informational))
                Logger.Log(WriteEntry((EventLogLevel)EventLevel.Informational, null, message, args));
        }

        /// <summary>
        /// Logs a verbose event.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogVerbose(string message, params object[] args)
        {
            if (IdentityModelEventSource.Logger.IsEnabled())
                IdentityModelEventSource.Logger.WriteVerbose(message, args);

            if (Enum.IsDefined(typeof(EventLogLevel), (int)EventLevel.Verbose) && Logger.IsEnabled((EventLogLevel)EventLevel.Verbose))
                Logger.Log(WriteEntry((EventLogLevel)EventLevel.Verbose, null, message, args));
        }

        /// <summary>
        /// Logs a warning event.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogWarning(string message, params object[] args)
        {
            if (IdentityModelEventSource.Logger.IsEnabled())
                IdentityModelEventSource.Logger.WriteWarning(message, args);

            if (Enum.IsDefined(typeof(EventLogLevel), (int)EventLevel.Warning) && Logger.IsEnabled((EventLogLevel)EventLevel.Warning))
                Logger.Log(WriteEntry((EventLogLevel)EventLevel.Warning, null, message, args));
        }

        /// <summary>
        /// Logs an exception using the event source logger and returns new typed exception.
        /// </summary>
        /// <param name="eventLevel">Identifies the level of an event to be logged.</param>
        /// <param name="argumentName">Identifies the argument whose value generated the ArgumentException.</param>
        /// <param name="innerException">the inner <see cref="Exception"/> to be added to the outer exception.</param>
        /// <param name="format">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        private static T LogExceptionImpl<T>(EventLevel eventLevel, string argumentName, Exception innerException, string format, params object[] args) where T : Exception 
        {
            string message = null;

            if (args != null)
                message = string.Format(CultureInfo.InvariantCulture, format, args);
            else
                message = format;

            if (IdentityModelEventSource.Logger.IsEnabled() && IdentityModelEventSource.Logger.LogLevel >= eventLevel)
                IdentityModelEventSource.Logger.Write(eventLevel, innerException, message);

            EventLogLevel eventLogLevel = Enum.IsDefined(typeof(EventLogLevel), (int)eventLevel) ? (EventLogLevel)eventLevel : EventLogLevel.Error;
            if (Logger.IsEnabled(eventLogLevel))
                Logger.Log(WriteEntry((EventLogLevel)eventLevel, innerException, message, null));

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

            return string.Format(CultureInfo.InvariantCulture, format, args);
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

            // Logs basic information (library version, DateTime, whether PII is ON/OFF) once before any log messages are written.
            if (!_isHeaderWritten)
            {
                string headerMessage = string.Format(
                    CultureInfo.InvariantCulture,
                    "Microsoft.IdentityModel Version: {0}. Date {1}. {2}",
                    typeof(IdentityModelEventSource).Assembly.GetName().Version.ToString(),
                    DateTime.UtcNow,
                    IdentityModelEventSource.ShowPII ? _piiOnLogMessage : _piiOffLogMessage);

                LogEntry headerEntry = new LogEntry();
                headerEntry.EventLogLevel = EventLogLevel.LogAlways;
                headerEntry.Message = headerMessage;
                Logger.Log(headerEntry);

                _isHeaderWritten = true;
            }

            LogEntry entry = new LogEntry();
            entry.EventLogLevel = eventLogLevel;
            entry.Message = message;

            return entry;
        }
    }
}
