// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Abstractions
{
    /// <summary>
    /// Defines Event Log Levels.
    /// </summary>
    public enum EventLogLevel
    {
        /// <summary>
        /// No level filtering is done on this log level. Log messages of all levels will be logged.
        /// </summary>
        LogAlways = 0,

        /// <summary>
        /// Logs that describe an unrecoverable application or system crash, or a catastrophic failure that requires
        /// immediate attention.
        /// </summary>
        Critical = 1,

        /// <summary>
        /// Logs that highlight when the current flow of execution is stopped due to a failure. These should indicate a
        /// failure in the current activity, not an application-wide failure.
        /// </summary>
        Error = 2,

        /// <summary>
        /// Logs that highlight an abnormal or unexpected event in the application flow, but do not otherwise cause the
        /// application execution to stop.
        /// </summary>
        Warning = 3,

        /// <summary>
        /// Logs that track the general flow of the application. These logs should have long-term value.
        /// </summary>
        Informational = 4,

        /// <summary>
        /// Logs that are used for interactive investigation during development. These logs should primarily contain
        /// information useful for debugging and have no long-term value.
        /// </summary>
        Verbose = 5
    }
}
