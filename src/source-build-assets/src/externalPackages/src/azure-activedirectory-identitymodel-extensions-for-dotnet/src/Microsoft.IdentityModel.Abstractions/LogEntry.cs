// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Abstractions
{
    /// <summary>
    /// Defines the structure of a log entry.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Defines the <see cref="EventLogLevel"/>.
        /// </summary>
        public EventLogLevel EventLogLevel { get; set; }

        /// <summary>
        /// Message to be logged.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// A unique identifier for a request that can help with diagnostics across components.
        /// </summary>
        /// <remarks>
        /// Also referred to as ActivityId in Microsoft.IdentityModel.Tokens.CallContext.
        /// </remarks>
        public string? CorrelationId { get; set; }
    }
}
