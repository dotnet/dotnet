// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    /// <summary>
    /// Failure due to an invalid cache.
    /// </summary>
    public abstract class InvalidCacheProtocolException : FatalProtocolException
    {
        public InvalidCacheProtocolException(string message)
            : base(message)
        {
        }

        public InvalidCacheProtocolException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
