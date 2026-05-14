// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class HttpSourceException : FatalProtocolException
    {
        public HttpSourceException(string message)
            : base(message)
        {
        }
        public HttpSourceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
