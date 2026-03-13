// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;
using NuGet.Packaging.Core;

namespace NuGet.Packaging
{
    public class UnsafePackageEntryException : PackagingException, ILogMessageException
    {
        public UnsafePackageEntryException(string message) :
            base(message)
        {
        }

        public override ILogMessage AsLogMessage()
        {
            return LogMessage.CreateError(NuGetLogCode.NU1402, Message);
        }
    }
}
