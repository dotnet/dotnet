// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Abstractions
{
    /// <summary>
    /// Interface that needs to be implemented by classes providing logging in Microsoft identity libraries.
    /// </summary>
    public interface IIdentityLogger
    {
        /// <summary>
        /// Checks to see if logging is enabled at given <paramref name="eventLogLevel"/>.
        /// </summary>
        /// <param name="eventLogLevel">Log level of a message.</param>
        bool IsEnabled(EventLogLevel eventLogLevel);

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <param name="entry">Defines a structured message to be logged at the provided <see cref="LogEntry.EventLogLevel"/>.</param>
        void Log(LogEntry entry);
    }
}
