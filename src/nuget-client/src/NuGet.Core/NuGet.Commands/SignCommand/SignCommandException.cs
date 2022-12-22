// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.Commands
{
    /// <summary>
    /// Holds an <see cref="ILogMessage"/> and returns the message for the exception.
    /// </summary>
    public sealed class SignCommandException : Exception, ILogMessageException
    {
        private readonly ILogMessage _logMessage;

        public SignCommandException(ILogMessage logMessage)
            : base(logMessage?.Message)
        {
            _logMessage = logMessage ?? throw new ArgumentNullException(nameof(logMessage));
        }

        public ILogMessage AsLogMessage()
        {
            return _logMessage;
        }
    }
}
