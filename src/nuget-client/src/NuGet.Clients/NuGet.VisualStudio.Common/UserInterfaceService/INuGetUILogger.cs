// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary> UI logger abstraction. </summary>
    public interface INuGetUILogger
    {
        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="level">log level</param>
        /// <param name="message">log message</param>
        /// <param name="args">arguments for a string.Format(...) call on <paramref name="message"/> </param>
        void Log(ProjectManagement.MessageLevel level, string message, params object[] args);

        /// <summary> Log a message. </summary>
        /// <param name="message"> Log message. </param>
        /// <remarks>If the message has a log level of warning or error,
        /// the message is also logged to the Error List.</remarks>
        void Log(ILogMessage message);

        /// <summary> Report an error or warning. </summary>
        /// <param name="message"> Error or warning log message. </param>
        void ReportError(ILogMessage message);

        /// <summary> Start the logging. </summary>
        void Start();

        /// <summary> End the logging. </summary>
        void End();
    }
}
