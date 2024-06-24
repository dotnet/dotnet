// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Abstractions;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains information so that logs can be written when needed.
    /// </summary>
    internal class LogDetail
    {
        /// <summary>
        /// Creates an instance of <see cref="LogDetail"/>
        /// </summary>
        /// <paramref name="messageDetail"/> contains information about the exception that is used to generate the exception message.
        /// <paramref name="eventLogLevel"/> is the level of the event log.
        public LogDetail(MessageDetail messageDetail, EventLogLevel eventLogLevel)
        {
            EventLogLevel = eventLogLevel;
            MessageDetail = messageDetail;
        }

        /// <summary>
        /// Gets the level of the event log.
        /// </summary>
        public EventLogLevel EventLogLevel { get; }

        /// <summary>
        /// Gets the message detail.
        /// </summary>
        public MessageDetail MessageDetail { get; }
    }
}
