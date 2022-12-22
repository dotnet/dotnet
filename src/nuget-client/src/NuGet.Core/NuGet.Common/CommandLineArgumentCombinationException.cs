// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Common
{
    public class CommandLineArgumentCombinationException : Exception, ILogMessageException
    {
        private readonly ILogMessage _logMessage;

        public CommandLineArgumentCombinationException(string message)
            : base(message)
        {
            _logMessage = LogMessage.CreateError(NuGetLogCode.NU1000, message);
        }

        public virtual ILogMessage AsLogMessage()
        {
            return _logMessage;
        }
    }
}
