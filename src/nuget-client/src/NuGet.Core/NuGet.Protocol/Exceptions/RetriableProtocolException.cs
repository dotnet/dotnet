// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Core.Types
{
    public class RetriableProtocolException : NuGetProtocolException
    {
        public RetriableProtocolException(string message) : base(message)
        {
        }

        public RetriableProtocolException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
