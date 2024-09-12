// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

#if IS_DESKTOP
using System.Runtime.Serialization;
#endif

namespace NuGet.Credentials
{
    /// <summary>
    /// PluginUnexpectedStatusException results when a plugin credential provider returns an unexpected status,
    /// one not enumerated in PluginCredentialResponseExitCode.
    /// This typically occurs when a plugin throws a terminating exception.
    /// </summary>
    [Serializable]
    public class PluginUnexpectedStatusException : PluginException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginUnexpectedStatusException"/> class.
        /// </summary>
        public PluginUnexpectedStatusException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginUnexpectedStatusException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PluginUnexpectedStatusException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginUnexpectedStatusException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public PluginUnexpectedStatusException(string message, Exception inner) : base(message, inner) { }

#if IS_DESKTOP
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginUnexpectedStatusException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected PluginUnexpectedStatusException(
          SerializationInfo info,
          StreamingContext context) : base(info, context)
        { }
#endif

        /// <summary>
        /// Creates a new <see cref="PluginUnexpectedStatusException"/> with a formatted message indicating an unexpected status.
        /// </summary>
        /// <param name="path">The path of the plugin.</param>
        /// <param name="status">The unexpected status returned by the plugin.</param>
        /// <returns>A new instance of <see cref="PluginUnexpectedStatusException"/>.</returns>
        public static PluginException CreateUnexpectedStatusMessage(
            string path, PluginCredentialResponseExitCode status)
        {
            return new PluginUnexpectedStatusException(
                string.Format(CultureInfo.CurrentCulture, Resources.PluginException_UnexpectedStatus_Format, path, status));
        }
    }
}
