// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Exceptions that are generated while creating a package timestamp.
    /// </summary>
    public class TimestampException : SignatureException
    {
        public TimestampException()
            : base(string.Empty)
        {
        }

        public TimestampException(NuGetLogCode code, string message)
            : base(code, message)
        {
        }

        public TimestampException(NuGetLogCode code, string message, Exception innerException)
            : base(code, message, innerException)
        {
        }

        public TimestampException(string message)
            : this(NuGetLogCode.NU3000, message)
        {
        }
    }
}
