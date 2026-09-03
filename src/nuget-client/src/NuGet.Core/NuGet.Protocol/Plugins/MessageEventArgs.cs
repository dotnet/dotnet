// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Message event arguments.
    /// </summary>
    public sealed class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the message.
        /// </summary>
        public Message Message { get; }

        /// <summary>
        /// Instantiates a new <see cref="MessageEventArgs" /> class.
        /// </summary>
        /// <param name="message">A message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message" /> is <see langword="null" />.</exception>
        public MessageEventArgs(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Message = message;
        }
    }
}
