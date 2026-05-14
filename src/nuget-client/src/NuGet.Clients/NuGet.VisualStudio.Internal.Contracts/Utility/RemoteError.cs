// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public sealed class RemoteError
    {
        public ILogMessage LogMessage { get; }
        public IReadOnlyList<ILogMessage>? LogMessages { get; }
        public string? ProjectContextLogMessage { get; }
        public string? ActivityLogMessage { get; }
        public string TypeName { get; }

        public RemoteError(string typeName, ILogMessage logMessage, IReadOnlyList<ILogMessage>? logMessages)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(typeName));
            }

            TypeName = typeName;
            LogMessage = logMessage ?? throw new ArgumentNullException(nameof(logMessage));
            LogMessages = logMessages;
        }

        public RemoteError(
            string typeName,
            ILogMessage logMessage,
            IReadOnlyList<ILogMessage>? logMessages,
            string? projectContextLogMessage,
            string? activityLogMessage)
            : this(typeName, logMessage, logMessages)
        {
            ProjectContextLogMessage = projectContextLogMessage;
            ActivityLogMessage = activityLogMessage;
        }
    }
}
