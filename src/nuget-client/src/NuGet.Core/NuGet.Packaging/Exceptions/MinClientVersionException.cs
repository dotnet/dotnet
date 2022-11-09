// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;
using NuGet.Packaging.Core;

namespace NuGet.Packaging
{
    /// <summary>
    /// Custom exception type for a package that has a higher minClientVersion than the current client.
    /// </summary>
    public class MinClientVersionException : PackagingException, ILogMessageException
    {
        public MinClientVersionException(string message)
            : base(message)
        {
        }

        public override ILogMessage AsLogMessage()
        {
            return LogMessage.CreateError(NuGetLogCode.NU1401, Message);
        }
    }
}
