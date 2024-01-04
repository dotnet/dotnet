// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit.Abstractions;

namespace Xunit
{
    class _DiagnosticMessage : IDiagnosticMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticMessage"/> class.
        /// </summary>
        /// <param name="message">The message to send</param>
        public _DiagnosticMessage(string message)
        {
            Message = message;
        }

        /// <inheritdoc/>
        public string Message { get; set; }
    }
}
