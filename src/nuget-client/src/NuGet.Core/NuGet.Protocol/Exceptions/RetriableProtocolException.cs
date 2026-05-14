// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace NuGet.Protocol.Core.Types
{
    [Serializable]
    public class RetriableProtocolException : NuGetProtocolException
    {
        public RetriableProtocolException(string message) : base(message)
        {
        }

        public RetriableProtocolException(string message, Exception innerException) : base(message, innerException)
        {
        }

#if NET8_0_OR_GREATER
        [Obsolete(DiagnosticId = "SYSLIB0051")] // https://github.com/dotnet/docs/issues/34893
#endif
        protected RetriableProtocolException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
