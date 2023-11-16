// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A plugin exception.
    /// </summary>
    [Serializable]
    public sealed class PluginException : Exception
    {
        /// <summary>
        /// Instantiates a new <see cref="PluginException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public PluginException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="PluginException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PluginException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if NET8_0_OR_GREATER
        [Obsolete(DiagnosticId = "SYSLIB0051")] // https://github.com/dotnet/docs/issues/34893
#endif
        private PluginException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
