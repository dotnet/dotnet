// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A plugin protocol exception.
    /// </summary>
    [Serializable]
    public sealed class ProtocolException : Exception
    {
        /// <summary>
        /// Instantiates a new <see cref="ProtocolException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ProtocolException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="ProtocolException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ProtocolException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if NET8_0_OR_GREATER
        [Obsolete(DiagnosticId = "SYSLIB0051")] // https://github.com/dotnet/docs/issues/34893
#endif
        private ProtocolException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
