// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace NuGet.Credentials
{
    /// <summary>
    /// The exception thrown when a plugin credential provider cannot be started or returns an invalid response.
    /// </summary>
    [Serializable]
    public class PluginException : Exception
    {
        private const string RedactedPassword = "********";

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginException"/> class.
        /// </summary>
        public PluginException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginException"/> class with an error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PluginException(string? message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginException"/> class with an error message and inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that caused the current exception.</param>
        public PluginException(string? message, Exception? inner) : base(message, inner) { }
#if IS_DESKTOP
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that contains the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected PluginException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
#endif
        /// <summary>
        /// Creates an exception for an error raised while executing a plugin credential provider.
        /// </summary>
        /// <param name="path">The plugin path.</param>
        /// <param name="inner">The exception raised while executing the plugin.</param>
        /// <returns>A plugin exception describing the execution error.</returns>
        public static PluginException Create(string path, Exception inner)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (inner == null)
            {
                throw new ArgumentNullException(nameof(inner));
            }

            return new PluginException(
                string.Format(CultureInfo.CurrentCulture, Resources.PluginException_Exception_Format, path, inner.GetType().Name),
                inner);
        }

        /// <summary>
        /// Creates an exception indicating that a plugin credential provider timed out.
        /// </summary>
        /// <param name="path">The plugin path.</param>
        /// <param name="timeoutMillis">The timeout, in milliseconds.</param>
        /// <returns>A plugin exception describing the timeout.</returns>
        public static PluginException CreateTimeoutMessage(string path, int timeoutMillis)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return new PluginException(
                string.Format(CultureInfo.CurrentCulture, Resources.PluginException_Timeout_Format, path, timeoutMillis));
        }

        /// <summary>
        /// Creates an exception indicating that a plugin credential provider could not be started.
        /// </summary>
        /// <param name="path">The plugin path.</param>
        /// <returns>A plugin exception describing the start failure.</returns>
        public static PluginException CreateNotStartedMessage(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return new PluginException(string.Format(CultureInfo.CurrentCulture, Resources.PluginException_NotStarted_Format, path));
        }

        /// <summary>
        /// Creates an exception indicating that a plugin credential provider path could not be found.
        /// </summary>
        /// <param name="path">The configured plugin path.</param>
        /// <param name="attempted">The path that was attempted.</param>
        /// <returns>A plugin exception describing the missing path.</returns>
        [Obsolete("This method is unused and will be removed in a future release.")]
        public static PluginException CreatePathNotFoundMessage(string path, string attempted)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (attempted == null)
            {
                throw new ArgumentNullException(nameof(attempted));
            }

            return new PluginException(string.Format(CultureInfo.CurrentCulture, Resources.PluginException_PathNotFound_Format, path,
                attempted));
        }

        /// <summary>
        /// Creates an exception indicating that a plugin credential provider aborted the request.
        /// </summary>
        /// <param name="path">The plugin path.</param>
        /// <param name="message">The message returned by the plugin, if available.</param>
        /// <returns>A plugin exception describing the aborted request.</returns>
        public static PluginException CreateAbortMessage(string path, string? message)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return new PluginException(string.Format(CultureInfo.CurrentCulture, Resources.PluginException_Abort_Format, path, message));
        }

        /// <summary>
        /// Creates an exception indicating that a plugin credential provider returned an unreadable response.
        /// </summary>
        /// <param name="path">The plugin path.</param>
        /// <param name="status">The exit code returned by the plugin.</param>
        /// <returns>A plugin exception describing the unreadable response.</returns>
        public static PluginException CreateUnreadableResponseExceptionMessage(
            string path,
            PluginCredentialResponseExitCode status)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return new PluginException(string.Format(
                CultureInfo.CurrentCulture,
                Resources.PluginException_UnreadableResponse_Format,
                path,
                status));
        }

        /// <summary>
        /// Creates an exception indicating that a plugin credential provider returned an invalid response.
        /// </summary>
        /// <param name="path">The plugin path.</param>
        /// <param name="status">The exit code returned by the plugin.</param>
        /// <param name="response">The invalid response returned by the plugin.</param>
        /// <returns>A plugin exception describing the invalid response.</returns>
        public static PluginException CreateInvalidResponseExceptionMessage(
            string path,
            PluginCredentialResponseExitCode status,
            PluginCredentialResponse response)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return new PluginException(string.Format(
                CultureInfo.CurrentCulture,
                Resources.PluginException_InvalidResponse_Format,
                path,
                status,
                response.Username,
                response.Password == null ? string.Empty : RedactedPassword,
                response.AuthTypes == null ? string.Empty : string.Join(", ", response.AuthTypes),
                response.Message));
        }
    }
}
