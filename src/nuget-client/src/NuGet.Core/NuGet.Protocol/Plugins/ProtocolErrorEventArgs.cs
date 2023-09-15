// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Protocol error event arguments.
    /// </summary>
    public sealed class ProtocolErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public Message Message { get; }

        /// <summary>
        /// Instantiates a new <see cref="ProtocolErrorEventArgs" /> class.
        /// </summary>
        /// <param name="exception">An exception.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception" /> is <see langword="null" />.</exception>
        public ProtocolErrorEventArgs(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Exception = exception;
        }

        /// <summary>
        /// Instantiates a new <see cref="ProtocolErrorEventArgs" /> class.
        /// </summary>
        /// <param name="exception">An exception.</param>
        /// <param name="message">A message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message" /> is <see langword="null" />.</exception>
        public ProtocolErrorEventArgs(Exception exception, Message message)
            : this(exception)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Message = message;
        }
    }
}
