// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;
using NuGet.Common;

namespace NuGet.Protocol.Core.Types
{
    [Serializable]
    public class FatalProtocolException : NuGetProtocolException
    {
        public FatalProtocolException(string message) : base(message)
        {
        }

        public FatalProtocolException(string message, Exception innerException) : base(message, innerException)
        {
        }

#if NET8_0_OR_GREATER
        [Obsolete(DiagnosticId = "SYSLIB0051")] // https://github.com/dotnet/docs/issues/34893
#endif
        protected FatalProtocolException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
