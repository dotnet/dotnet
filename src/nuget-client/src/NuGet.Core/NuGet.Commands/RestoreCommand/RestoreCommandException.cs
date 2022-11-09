// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.Commands
{
    /// <summary>
    /// Holds an <see cref="IRestoreLogMessage"/> and returns the message for the exception.
    /// </summary>
    public class RestoreCommandException : Exception, ILogMessageException
    {
        private readonly IRestoreLogMessage _logMessage;

        public RestoreCommandException(IRestoreLogMessage logMessage)
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
