// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace Microsoft.Build.NuGetSdkResolver.Test
{
    /// <summary>
    /// A mock implementation of <see cref="SdkLogger" /> that stores logged messages.
    /// </summary>
    public class MockSdkLogger : SdkLogger
    {
        /// <summary>
        /// Stores the list of messages that have been logged.
        /// </summary>
        private readonly List<(string Message, MessageImportance Importance)> _messages = new List<(string Message, MessageImportance Importance)>();

        /// <summary>
        /// Gets a list of messages that have been logged.
        /// </summary>
        public IReadOnlyCollection<(string Message, MessageImportance Importance)> LoggedMessages => _messages;

        /// <inheritdoc cref="SdkLogger.LogMessage(string, MessageImportance)" />
        public override void LogMessage(string message, MessageImportance messageImportance = MessageImportance.Low)
        {
            _messages.Add((message, messageImportance));
        }
    }
}
