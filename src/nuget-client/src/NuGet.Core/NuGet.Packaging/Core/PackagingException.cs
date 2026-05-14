// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.Packaging.Core
{
    public class PackagingException : Exception, ILogMessageException
    {
        private readonly IPackLogMessage _logMessage;

        public PackagingException(string message)
            : base(message)
        {
            _logMessage = PackagingLogMessage.CreateError(message, NuGetLogCode.NU5000);
        }

        public PackagingException(NuGetLogCode logCode, string message)
            : base(message)
        {
            _logMessage = PackagingLogMessage.CreateError(message, logCode);
        }

        public PackagingException(NuGetLogCode logCode, string message, Exception innerException)
            : base(message, innerException)
        {
            _logMessage = PackagingLogMessage.CreateError(message, logCode);
        }

        public PackagingException(string message, Exception innerException)
            : base(message, innerException)
        {
            _logMessage = PackagingLogMessage.CreateError(message, NuGetLogCode.NU5000);
        }

        public virtual ILogMessage AsLogMessage()
        {
            return _logMessage;
        }
    }
}
